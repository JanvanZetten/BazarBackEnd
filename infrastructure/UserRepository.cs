using Core.Domain;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly BazarContext _ctx;

        public UserRepository(BazarContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Returns the size of the Users table.
        /// </summary>
        public int Count()
        {
            return _ctx.Users.Count();
        }

        /// <summary>
        /// Creates an already prepared user in the table. Password needs to have been set and encrypted.
        /// </summary>
        public User Create(User user)
        {
            var item = _ctx.Users.Add(user).Entity;
            _ctx.SaveChanges();
            return item;
        }

        /// <summary>
        /// Deletes a user from the table with the given id.
        /// </summary>
        public User Delete(int id)
        {
            var user = GetById(id);
            if (user == null)
                return null;
            _ctx.Users.Remove(user);
            _ctx.SaveChanges();
            return user;
        }

        /// <summary>
        /// Gets all users from the table.
        /// </summary>
        public IEnumerable<User> GetAll()
        {
            return _ctx.Users.ToList();
        }

        /// <summary>
        /// Gets a user from the table with the specified id.
        /// </summary>
        public User GetById(int id)
        {
            return _ctx.Users.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Prepares a user for creation and encrypts the password. Adds user to the table through the Create method.
        /// </summary>
        public User Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Create(user);
        }

        /// <summary>
        /// Checks the table if a username already exists. Returns true if the username doesn't already exist in the table.
        /// </summary>
        public bool UniqueUsername(string username)
        {
            return _ctx.Users.Any(u => u.Username == username);
        }

        /// <summary>
        /// Updates the user in the table. Only username is updated.
        /// </summary>
        public User Update(User user)
        {
            var oldUser = GetById(user.Id);
            if (oldUser == null)
                return null;

            oldUser.Username = user.Username;

            var item = _ctx.Users.Update(oldUser).Entity;
            _ctx.SaveChanges();
            return item;
        }

        /// <summary>
        /// Generates a hash and salt to the given password to allow for encryption.
        /// </summary>
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
