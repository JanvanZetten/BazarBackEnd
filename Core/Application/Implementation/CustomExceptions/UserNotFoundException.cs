using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("Brugeren blev ikke fundet") { }

        public UserNotFoundException(string message) : base(message) { }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
