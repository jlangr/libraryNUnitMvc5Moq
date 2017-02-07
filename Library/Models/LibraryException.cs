using System;

namespace Library.Models
{
    public class LibraryException: ApplicationException
    {
        public LibraryException(string message) : base(message)
        {
        }
    }
}
