using System;
using System.Collections.Generic;

namespace ElefanteLetrado.BotReader.Events
{
    public class AggregateEvents : ILibraryEvent
    {
        private readonly List<ILibraryEvent> _events = new List<ILibraryEvent>();

        public AggregateEvents AddEvent(ILibraryEvent libraryEvent)
        {
            _events.Add(libraryEvent);
            return this;
        }

        public void OnEnterLevel(string levelName)
        {
            DoEvent(e => e.OnEnterLevel(levelName));
        }

        public void OnLeaveLevel(string levelName)
        {
            DoEvent(e => e.OnLeaveLevel(levelName));
        }

        public void OnEnterBook(string bookTitle)
        {
            DoEvent(e => e.OnEnterBook(bookTitle));
        }

        public void OnLeaveBook(string bookTitle)
        {
            DoEvent(e => e.OnLeaveBook(bookTitle));
        }

        public void OnEnterBookLoading(string bookTitle)
        {
            DoEvent(e => e.OnEnterBookLoading(bookTitle));
        }

        public void OnLeaveBookLoading(string bookTitle)
        {
            DoEvent(e => e.OnLeaveBookLoading(bookTitle));
        }

        public void OnReadAllBooks()
        {
            DoEvent(e => e.OnReadAllBooks());
        }

        private void DoEvent(Action<ILibraryEvent> method)
        {
            foreach (var item in _events)
            {
                method(item);
            }
        }
    }
}