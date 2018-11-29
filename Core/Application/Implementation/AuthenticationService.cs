using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entity;
using Microsoft.IdentityModel.Tokens;

namespace Core.Application.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private byte[] secretBytes;

        public AuthenticationService(byte[] secret)
        {
            secretBytes = secret;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

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
    }
}
