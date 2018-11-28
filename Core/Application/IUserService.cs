using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface IUserService
    {
        User Create(User user);
        IEnumerable<User> GetAll();
        User GetByID(int id);
        User Update(User user);
        User Delete(int id);
    }
}
