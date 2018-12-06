using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class NotUniqueUsernameException : Exception
    {
        public NotUniqueUsernameException()  : base("Username is already taken")
        {

        }

        public NotUniqueUsernameException(string message) : base(message)
        {
        }

        public NotUniqueUsernameException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
