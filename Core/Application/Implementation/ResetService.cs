using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Application.Implementation
{
    public class ResetService : IResetService
    {
        private readonly IResetRepository _repo;
        private readonly IAuthenticationService _auth;
        private readonly IUserRepository _userRepo;

        public ResetService(IResetRepository repo, IAuthenticationService auth, IUserRepository userRepo)
        {
            _repo = repo;
            _auth = auth;
            _userRepo = userRepo;
        }

        /// <summary>
        /// Resets the entire program by removing users from all booths.
        /// IMPORTANT: Requires further expansion as more functionality is required.
        /// Is meant to reset the entire thing.
        /// </summary>
        /// <param name="token">Token to verify that a user is admin</param>
        /// <returns>
        /// Number of objects that have been affected
        /// </returns>
        public int ResetAll(string token)
        {
            var verifyToken = _auth.VerifyUserFromToken(token);
            var user = _userRepo.GetAll().FirstOrDefault(u => u.Username == verifyToken);

            // Checks if the user that attempts to reset exists
            if(user == null)
            {
                throw new UserNotFoundException();
            }

            // Checks if the user that attempts to reset is an admin
            if(user.IsAdmin == false)
            {
                throw new NotAllowedException();
            }

            return _repo.Reset();
        }
    }
}
