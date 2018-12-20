using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class IncompatibleFileTypeException : Exception
    {
        public IncompatibleFileTypeException() : base("Filtypen er ikke kompatibel.") { }

        public IncompatibleFileTypeException(string message) : base(message) { }

        public IncompatibleFileTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
