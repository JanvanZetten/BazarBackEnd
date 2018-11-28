using System;
using System.Collections.Generic;
using System.Text;
using Core.Domain;
using Core.Entity;

namespace Core.Application.Implementation
{
    public class UserService : IUserService
    {
        readonly IUserRepository _userRepository;

        public UserService(IUserRepository UserRepository) 
        {
            _userRepository = UserRepository;
        }
            
        public User Create(User user, string password)
        {
            if(_userRepository.UniqueUsername(user.Username))
            {

            }
            else
            {
                throw new ArgumentException("Username is already taken.");
            }
        }

        public User Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public User Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
