using System;
using System.Threading;
using ElefanteLetrado.BotReader.Events;
using ElefanteLetrado.BotReader.Pages;

namespace ElefanteLetrado.BotReader.Scripts
{
    public class SequentialReadScript
    {
        private readonly int _stayEachPage = 1000;
        private readonly LoginPage _login;
        private readonly StudentPage _student;
        private readonly ReaderPage _reader;
        private ILibraryEvent _events = EmptyEvent.Instance;

        public SequentialReadScript(LoginPage login, StudentPage student, ReaderPage reader)
        {
            _login = login;
            _student = student;
            _reader = reader;
        }

        public string ContinueFromLevel { get; set; }

        public string ContinueFromBook { get; set; }

        public ILibraryEvent Events
        {
            get { return _events; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Events does not null.");
                }
                _events = value;
            }
        }

        public void PlayWith(StudentLoginInfo student)
        {
            _login.submitUserLogin(student.TeacherLogin);
            _login.submitStudent(student.CourseName, student.StudentName, student.StudentPassword);

            _student.WaitShowingLevels(); //// info: esperar carregar os níveis antes de tentar ler eles
            string[] levelNames = _student.GetAllActiveLevels();

            for (var levelNameIndex = GetIndexOf(levelNames, ContinueFromLevel); levelNameIndex < levelNames.Length; levelNameIndex++)
            {
                var levelName = levelNames[levelNameIndex];

                _student.WaitShowingBookshelf(); //// info: esperar os livros estarem visivelmente na prateleira para o loader ser encerrado
                _student.ClickLevelByName(levelName);
                _events.OnEnterLevel(levelName);

                _student.WaitShowingBookshelf(); //// info: esperar os livros estarem visivelmente na prateleira
                string[] bookNames = _student.GetAllBookNames();

                for (var bookNameIndex = GetIndexOf(bookNames, ContinueFromBook); bookNameIndex < bookNames.Length; bookNameIndex++)
                {
                    var bookName = bookNames[bookNameIndex];
                    _student.WaitShowingBookshelf(); //// info: esperar os livros estarem visivelmente na prateleira

                    _student.ClickBookByName(bookName);
                    _events.OnEnterBook(bookName);

                    _events.OnEnterBookLoading(bookName);
                    _reader.WaitLoadingBook();
                    _events.OnLeaveBookLoading(bookName);

                    //// TODO: esperar carregar o DOM do js antes para ter certeza de que a tela carregou todos os elementos de UI
                    Thread.Sleep(3000);

                    if (_reader.IsOpenModalContinue())
                    {
                        _reader.ClickContinueRead();
                    }

                    while (!_reader.HasReachLastPage())
                    {
                        Thread.Sleep(_stayEachPage); //// info: dar tempo para simular a ação de ler o livro
                        _reader.GoToNextPage();
                    }

                    //// info: esperar o envio do último tempo ao servidor e a resposta se foi aprovado ou não
                    _reader.WaitFinishBook();

                    _reader.ClickToFinishBook();
                    if (_reader.HasOpenModalFinishBook())
                    {
                        _reader.GoToLibrary();
                        _events.OnLeaveBook(bookName);
                    }
                }

                _events.OnLeaveLevel(levelName);

                if (levelNameIndex == levelNames.Length - 1)
                {
                    levelNameIndex = -1; //// info: manter um ciclo contínuo de leitura
                    _events.OnReadAllBooks();
                }
            }
        }

        private static int GetIndexOf(string[] items, string value)
        {
            var index = string.IsNullOrEmpty(value) ? 0 : Array.IndexOf(items, value);
            return index > -1 ? index : 0;
        }
    }
}