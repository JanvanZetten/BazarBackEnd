using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Application.Implementation.CustomExceptions
{
    public class BoothNotFoundException : Exception
    {
        public BoothNotFoundException(int id) : base($"Stand nr. {id} blev ikke fundet.")
        {
        }

        public BoothNotFoundException()  : base("Standen blev ikke fundet.")
        {
        }

        public BoothNotFoundException(string message) : base(message)
        {
        }

        public BoothNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
