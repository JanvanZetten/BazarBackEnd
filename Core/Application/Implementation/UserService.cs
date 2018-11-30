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
        readonly IAuthenticationService _authService;

        public UserService(IUserRepository UserRepository, IAuthenticationService authService) 
        {
            _userRepository = UserRepository;
            _authService = authService;
        }
            
        public User Create(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException("The user is null.");
            if (InputCheck.ValidLength("username", user.Username, 3, 40)
                && InputCheck.ValidLength("password", password, 8, 40)
                && InputCheck.ValidPassword(password)){}
            if (_userRepository.UniqueUsername(user.Username))
                throw new ArgumentException("Username is already taken.");

            byte[] passwordHash, passwordSalt;
            _authService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return _userRepository.Create(user);
        }

        public User Delete(int id)
        {
            User user = _userRepository.Delete(id);
            if (user == null)
                throw new ArgumentOutOfRangeException("User not found. No user has been deleted");
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetByID(int id)
        {
            User user = _userRepository.GetById(id);

            if (user == null)
                throw new ArgumentOutOfRangeException("Could not find the specified user.");

            return user;
        }

        public User Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException("The user is null.");
            if (InputCheck.ValidLength("username", user.Username, 3, 40)) { }

            User userOrg = _userRepository.GetById(user.Id);
            if(userOrg == null)
                throw new ArgumentOutOfRangeException("User not found. No user has been deleted");

            if (userOrg.Username.ToLower() != user.Username.ToLower())
            {
                if (!_userRepository.UniqueUsername(user.Username))
                    throw new ArgumentException("Username is already taken.");
            }

            return _userRepository.Update(user);
        }
    }
}
