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

        public BoothService(IRepository<User> UserRepository, IRepository<Booth> BoothRepository)
        {
            _userRepository = UserRepository;
            _boothRepository = BoothRepository;
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
