using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class AlreadyOnWaitingListException : Exception
    {
        public AlreadyOnWaitingListException() : base("Du er allerede på ventelisten.")
        {
        }

        public AlreadyOnWaitingListException(string message) : base(message)
        {
        }

        public AlreadyOnWaitingListException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
