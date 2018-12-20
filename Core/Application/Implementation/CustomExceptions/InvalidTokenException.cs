using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Din login session er udløbet eller invalid. Log venligst på igen.") { }

        public InvalidTokenException(string message) : base(message) { }

        public InvalidTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
