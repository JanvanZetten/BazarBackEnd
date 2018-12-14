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

        public int ResetAll(string token)
        {
            var verifyToken = _auth.VerifyUserFromToken(token);
            var user = _userRepo.GetAll().FirstOrDefault(u => u.Username == verifyToken);
            if(user == null)
            {
                throw new UserNotFoundException();
            }
            if(user.IsAdmin == false)
            {
                throw new NotAllowedException();
            }
            return _repo.Reset();
        }
    }
}
