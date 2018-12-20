using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class OnWaitingListException : Exception
    {
        public OnWaitingListException(string message) : base(message) { }

        public OnWaitingListException(string message, Exception innerException) : base(message, innerException) { }
    }
}
