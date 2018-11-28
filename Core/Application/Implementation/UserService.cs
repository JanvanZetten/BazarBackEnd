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
            if (user == null)
                throw new ArgumentNullException("The user is null.");
            if (InputCheck.ValidLength("username", user.Username, 3, 40)
                && InputCheck.ValidLength("password", password, 8, 40)
                && InputCheck.ValidPassword(password)){}
            if (!_userRepository.UniqueUsername(user.Username))
                throw new ArgumentException("Username is already taken.");

            return _userRepository.Register(user, password);
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
