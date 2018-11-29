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
            newBooth.Id = 0;
           return _BoothRepo.Create(newBooth);
        }

        public Booth Delete(int id)
        {
            GetById(id);
            return _BoothRepo.Delete(id);
        }

        public List<Booth> GetAll()
        {
            return _BoothRepo.GetAll().ToList();
        }

        public Booth GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be higher than 0");
            }
            var booth = _BoothRepo.GetById(id);
            if(booth == null)
            {
                throw new ArgumentOutOfRangeException("Booth with selected ID was not found.");
            }
            return booth;

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
