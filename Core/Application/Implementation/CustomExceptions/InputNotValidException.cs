using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class InputNotValidException : Exception
    {

        public InputNotValidException(string message) : base(message) { }

        public InputNotValidException(string message, Exception innerException) : base(message, innerException) { }

    }
}
