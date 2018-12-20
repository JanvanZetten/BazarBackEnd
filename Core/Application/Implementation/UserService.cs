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

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">The user to be created, minus password</param>
        /// <param name="password">The password of the user</param>
        /// <returns></returns>
        public User Create(User user, string password)
        {
            // Checks if the user is an object
            if (user == null)
                throw new UserNotFoundException("Brugeren kan ikke være tomt.");

            // Checks if the user has a valid password and username, remember to change in UPDATE method as well
                // Username must be 3 - 40 characters long
            if (InputCheck.ValidLength("brugernavn", user.Username, 3, 40)
                // Password must be 8 - 40 characters long
                && InputCheck.ValidLength("kodeord", password, 8, 40)
                // Password must contain a small and capital letter and a number
                && InputCheck.ValidPassword(password)){}

            // Checks if the username is unique in the database
            if (!_userRepository.UniqueUsername(user.Username))
                throw new NotUniqueUsernameException();

            // Creates a new Hash and Salt for the user for security purposes
            byte[] passwordHash, passwordSalt;
            _authService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return _userRepository.Create(user);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to be deleted</param>
        public User Delete(int id)
        {
            User user = _userRepository.Delete(id);

            // Checks if the user exists
            if (user == null)
                throw new UserNotFoundException();

            //LOG
            _logService.Create($"Brugeren {user?.Username} (Id: {user?.Id}) er blevet slettet fra databasen");

            return user;
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        /// <summary>
        /// Returns a specific user
        /// </summary>
        /// <param name="id">The ID of the user</param>
        public User GetByID(int id)
        {
            User user = _userRepository.GetById(id);

            // Checks if the user exists
            if (user == null)
                throw new UserNotFoundException();

            return user;
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user">The new user info with the same ID as the one to be changed</param>
        public User Update(User user)
        {
            // Chcecks if the user is an object
            if (user == null)
                throw new UserNotFoundException();

            // Checks if the new username is valid, must has 3 - 40 characters, remember to change in CREATE method as well
            if (InputCheck.ValidLength("username", user.Username, 3, 40)) { }

            User userOrg = _userRepository.GetById(user.Id);

            // Checks if the user on the ID exists
            if (userOrg == null)
                throw new UserNotFoundException();

            // Checks if the new username is unique if it has changed
            if (userOrg.Username.ToLower() != user.Username.ToLower())
            {
                if (!_userRepository.UniqueUsername(user.Username))
                    throw new NotUniqueUsernameException();
            }

            return _userRepository.Update(user);
        }
    }
}
