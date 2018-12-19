using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class AlreadyBookedException : Exception
    {
        public AlreadyBookedException() : base("Denne stand er allerede reserveret.") { }

        public AlreadyBookedException(string message) : base(message) { }

        public AlreadyBookedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
