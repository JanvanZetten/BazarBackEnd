using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class LogNotFoundException : Exception
    {
        public LogNotFoundException() : base("Log blev ikke fundet.") { }

        public LogNotFoundException(string message) : base(message) { }

        public LogNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
