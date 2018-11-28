using Core.Domain;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace infrastructure
{
    public class UserRepository : IUserRepository
    {
        public int Count()
        {
            throw new NotImplementedException();
        }

        public User Create(User entity)
        {
            throw new NotImplementedException();
        }

        public User Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public User Register(User user, string password)
        {
            throw new NotImplementedException();
        }

        public bool UniqueUsername(string username)
        {
            throw new NotImplementedException();
        }

        public User Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
