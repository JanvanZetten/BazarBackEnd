using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface IAuthenticationService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPaswordHash(string password, byte[] storedHash, byte[] storedSalt);
        string GenerateToken(User user);
    }
}
