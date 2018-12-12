using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class EmptyBookingException : Exception
    {
        public EmptyBookingException() : base("Der blev ikke fundet nogle stande i reservationen.")
        {
        }

        public EmptyBookingException(string message) : base(message)
        {
        }

        public EmptyBookingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
