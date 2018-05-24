namespace ElefanteLetrado.BotReader.Events
{
    public class CheckpointEvent : ILibraryEvent
    {
        public string LastLevel { get; set; }

        public string LastBook { get; set; }

        public void OnEnterLevel(string levelName)
        {
            LastLevel = levelName;
        }

        public void OnLeaveLevel(string levelName) { }

        public void OnEnterBook(string bookTitle)
        {
            LastBook = bookTitle;
        }

        public void OnLeaveBook(string bookTitle) { }

        public void OnEnterBookLoading(string bookTitle) { }

        public void OnLeaveBookLoading(string bookTitle) { }

        public void OnReadAllBooks() { }
    }
}