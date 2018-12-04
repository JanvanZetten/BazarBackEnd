using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Core.Application.Implementation.CustomExceptions;
using Core.Entity;
using Microsoft.IdentityModel.Tokens;

namespace Core.Application.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private byte[] secretBytes;
        private TokenValidationParameters tokenVal;

        public AuthenticationService(byte[] secret, TokenValidationParameters tvp)
        {
            secretBytes = secret;
            tokenVal = tvp;
        }

        /// <summary>
        /// Generates a password salt based on the given password and a hash based on the generated salt.
        /// </summary>
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Generates a token for the user that logs in, based on their role. Uses secretBytes which are generated at random in 
        /// the startup file. The final number in the token object is the amount of time it takes before the token expires.
        /// </summary>
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Currently removed because IsAdmin isn't an attribute in User.
            /*
            if (user.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            */

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(secretBytes),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(null, null, claims.ToArray(), DateTime.Now, DateTime.Now.AddMinutes(10)));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Checks whether a given password is eligible by comparing hash values.
        /// </summary>
        /// <returns>
        /// True if the password hash value matches with the one in the table.
        /// False if the password hash value doesn't match.
        /// </returns>
        public bool VerifyPaswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Verifies token and returns username if valid.
        /// </summary>
        public string VerifyUserFromToken(string token)
        {
            SecurityToken validatedToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var claimsPrincipal = handler.ValidateToken(token, tokenVal, out validatedToken);
            var result = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (result == null)
                throw new InvalidTokenException("The token is not valid!");
            return result;
        }
    }
}
