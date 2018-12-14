using System;
using System.Collections.Generic;
using System.Text;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;

namespace Core.Application.Implementation
{
    public class UserService : IUserService
    {
        readonly IUserRepository _userRepository;
        readonly IAuthenticationService _authService;
        readonly ILogService _logService;

        public UserService(
            IUserRepository UserRepository,
            IAuthenticationService authService,
            ILogService logService)
        {
            _userRepository = UserRepository;
            _authService = authService;
            _logService = logService;
        }

        #region Obsolete constructors
        [Obsolete("Constructor is used for tests, but should not be used elsewhere.")]
        public UserService(
            IUserRepository UserRepository,
            IAuthenticationService authService) 
        {
            _userRepository = UserRepository;
            _authService = authService;
        }
        #endregion

        public User Create(User user, string password)
        {
            if (user == null)
                throw new UserNotFoundException("The user is null.");
            if (InputCheck.ValidLength("username", user.Username, 3, 40)
                && InputCheck.ValidLength("password", password, 8, 40)
                && InputCheck.ValidPassword(password)){}
            if (!_userRepository.UniqueUsername(user.Username))
                throw new NotUniqueUsernameException();

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
                throw new UserNotFoundException();

            //LOG
            _logService.Create($"Brugeren {user?.Username} (Id: {user?.Id}) er blevet slettet fra databasen");

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
                throw new UserNotFoundException();

            return user;
        }

        public User Update(User user)
        {
            if (user == null)
                throw new UserNotFoundException();
            if (InputCheck.ValidLength("username", user.Username, 3, 40)) { }

            User userOrg = _userRepository.GetById(user.Id);
            if (userOrg == null)
                throw new UserNotFoundException();

            if (userOrg.Username.ToLower() != user.Username.ToLower())
            {
                if (!_userRepository.UniqueUsername(user.Username))
                    throw new NotUniqueUsernameException();
            }

            return _userRepository.Update(user);
        }
    }
}
