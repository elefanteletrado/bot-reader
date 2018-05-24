using System;
using System.Diagnostics;
using System.IO;

namespace ElefanteLetrado.BotReader.Events
{
    public class LogEvent : ILibraryEvent
    {
        private readonly TextWriter _writer;
        private Stopwatch _loadingEllapsed = new Stopwatch();

        public LogEvent(TextWriter writer)
        {
            _writer = writer;
        }

        public void OnEnterLevel(string levelName)
        {
            WriteLine("Nível: {0}", levelName);
        }

        public void OnLeaveLevel(string levelName)
        {
            WriteLine("Saindo do nível: {0}", levelName);
        }

        public void OnEnterBook(string bookTitle)
        {
            WriteLine("Ler o livro {0}", bookTitle);
        }

        public void OnLeaveBook(string bookTitle)
        {
            WriteLine("Finalizou o livro");
        }

        public void OnEnterBookLoading(string bookTitle)
        {
            _loadingEllapsed.Reset();
            _loadingEllapsed.Start();
        }

        public void OnLeaveBookLoading(string bookTitle)
        {
            _loadingEllapsed.Stop();
            WriteLine("Carregou o livro em {0}", _loadingEllapsed.Elapsed);
        }

        public void OnReadAllBooks()
        {
            WriteLine("Terminou de ler todos os livros");
        }

        private void WriteLine(string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            var msg = string.Format("{0:dd/MM/yyyy hh:mm:ss.fff} {1}", DateTime.Now, message);
            _writer.WriteLine(msg);
        }
    }
}