using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public interface IUserRepository : IRepository<User>
    {
        bool UniqueUsername(string username);

        User Register(User user, string password);
    }
}
