using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class ImageURLNotFoundException : Exception
    {
        public ImageURLNotFoundException() : base("Billedet kunne ikke findes.") { }

        public ImageURLNotFoundException(string message) : base(message) { }

        public ImageURLNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
