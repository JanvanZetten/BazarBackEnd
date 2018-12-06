using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class NotOnWaitingListException : Exception
    {
        public NotOnWaitingListException() : base("Du er ikke på ventelisten.")
        {
        }

        public NotOnWaitingListException(string message) : base(message)
        {
        }

        public NotOnWaitingListException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
