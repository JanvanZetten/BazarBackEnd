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
                throw new ArgumentNullException("The " + fieldName + " cannot be null.");
            else if (str.Length < minVal)
                throw new ArgumentException("The " + fieldName + " is too short. A minimum of " + minVal + " characters are required.");
            else if (str.Length > maxVal)
                throw new ArgumentException("The " + fieldName + " is too long. A maximum of " + maxVal + " characters are required.");
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
                throw new ArgumentException("The password is required to have at least one uppercase letter.");
            if (!password.Any(char.IsLower))
                throw new ArgumentException("The password is required to have at least one lowercase letter.");
            if (!password.Any(char.IsDigit))
                throw new ArgumentException("The password is required to have at least one number.");
            return true;
        }
    }
}
