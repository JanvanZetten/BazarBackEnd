using System;
using System.Collections.Generic;
using System.Text;
using Core.Domain;
using Core.Entity;

namespace Core.Application.Implementation
{
    public class BoothService : IBoothService
    {
        readonly IRepository<User> _userRepository;
        readonly IRepository<Booth> _boothRepository;
        readonly IAuthenticationService _authenticationService;

        public BoothService(IRepository<User> userRepository, IRepository<Booth> boothRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _boothRepository = boothRepository;
            _authenticationService = authenticationService;
        }

        public Booth Book(string token)
        {
            throw new NotImplementedException();
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
