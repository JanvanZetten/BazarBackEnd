using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Domain;
using Core.Entity;

namespace Core.Application.Implementation
{
    public class BoothService : IBoothService
    {
        readonly IRepository<User> _userRepository;
        readonly IRepository<Booth> _boothRepository;
        readonly IAuthenticationService _authService;

        public BoothService(IRepository<User> userRepository, IRepository<Booth> boothRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _boothRepository = boothRepository;
            _authService = authenticationService;
        }

        public Booth Book(string token)
        {
            var username = _authService.VerifyUserFromToken(token);
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);

            if (user == null)
                throw new ArgumentOutOfRangeException("Could not find the specified user.");

            var booths = _boothRepository.GetAll();
            var booth = _boothRepository.GetAll().FirstOrDefault(b => b.Booker == null);
            if (booth == null)
                throw new InvalidOperationException("No booths available.");

            booth.Booker = user;
            return Update(booth);
        }

        public int CountAvalibleBooths()
        {
            throw new NotImplementedException();
        }

        public Booth Create(Booth newBooth)
        {
            throw new NotImplementedException();
        }

        public Booth Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Booth> GetAll()
        {
            throw new NotImplementedException();
        }

        public Booth GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Booth GetUsersBooking(int userId)
        {
            throw new NotImplementedException();
        }

        public Booth Update(Booth updatedBooth)
        {
            throw new NotImplementedException();
        }

        public int WaitingListPosition(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
