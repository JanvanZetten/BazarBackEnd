using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class NoBookingsFoundException : Exception
    {
        public NoBookingsFoundException() : base("Du har ingen reservation.") { }

        public NoBookingsFoundException(string message) : base(message) { }

        public NoBookingsFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
