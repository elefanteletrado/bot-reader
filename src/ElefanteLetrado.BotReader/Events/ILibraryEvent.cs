namespace ElefanteLetrado.BotReader.Events
{
    public interface ILibraryEvent
    {
        void OnEnterLevel(string levelName);

        void OnLeaveLevel(string levelName);

        void OnEnterBook(string bookTitle);

        void OnLeaveBook(string bookTitle);

        void OnEnterBookLoading(string bookTitle);

        void OnLeaveBookLoading(string bookTitle);

        void OnReadAllBooks();
    }
}