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

        private readonly IRepository<Booth> _BoothRepo;

        public BoothService(IRepository<Booth> repository){
            _BoothRepo = repository;
        }

        public Booth Book(string Username, string token)
        {
            throw new NotImplementedException();
        }

        public int CountAvalibleBooths()
        {
            return _BoothRepo.GetAll().Where(x => x.Booker == null).Count();
        }

        public Booth Create(Booth newBooth)
        {
            throw new NotImplementedException();
        }

        public Booth Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Booth> GetAll()
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
