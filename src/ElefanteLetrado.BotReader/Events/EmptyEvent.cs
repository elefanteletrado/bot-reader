namespace ElefanteLetrado.BotReader.Events
{
    public class EmptyEvent : ILibraryEvent
    {
        private static readonly EmptyEvent _instance = new EmptyEvent();

        private EmptyEvent() { }

        public static EmptyEvent Instance
        {
            get { return _instance; }
        }

        public void OnEnterLevel(string levelName) { }

        public void OnLeaveLevel(string levelName) { }
    
        public void OnEnterBook(string bookTitle) { }

        public void OnLeaveBook(string bookTitle) { }

        public void OnEnterBookLoading(string bookTitle) { }

        public void OnLeaveBookLoading(string bookTitle) { }

        public void OnReadAllBooks() { }
    }
}