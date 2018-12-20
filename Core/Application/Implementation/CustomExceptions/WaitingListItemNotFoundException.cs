using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class WaitingListItemNotFoundException : Exception
    {
        public WaitingListItemNotFoundException() : base("Reservationen på ventelisten blev ikke fundet.") { }

        public WaitingListItemNotFoundException(string message) : base(message) { }

        public WaitingListItemNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
