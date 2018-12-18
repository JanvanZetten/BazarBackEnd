using Core.Application.Implementation.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Application.Implementation
{
    public class InputCheck
    {
        /// <summary>
        /// Ths method checks for valid length of a given string. minVal and maxVal are used to determine the range.
        /// </summary>
        /// <returns>
        /// True if it passed all checks.
        /// </returns> 
        public static bool ValidLength(string fieldName, string str, int minVal, int maxVal)
        {
            if (str == null)
                throw new InputNotValidException("Feltet " + fieldName + " må ikke være tomt.");
            else if (str.Length < minVal)
                throw new InputNotValidException("Feltet " + fieldName + " er for kort. Det kan som minimum være " + minVal + " karakterer.");
            else if (str.Length > maxVal)
                throw new InputNotValidException("Feltet " + fieldName + " er for langt. Det kan som maximum være " + maxVal + " karakterer.");
            return true;
        }

        /// <summary>
        /// This method checks if a password contains all the required characters.
        /// </summary>
        /// <returns>
        /// True if it passed all checks.
        /// </returns>
        public static bool ValidPassword(string password)
        {
            if (!password.Any(char.IsUpper))
                throw new InputNotValidException("Kodeordet skal som minimum have et stort bogstav.");
            if (!password.Any(char.IsLower))
                throw new InputNotValidException("Kodeordet skal som minimum have et lille bogstav.");
            if (!password.Any(char.IsDigit))
                throw new InputNotValidException("Kodeordet skal som minimum have et nummer.");
            return true;
        }
    }
}
