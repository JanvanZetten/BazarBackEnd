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

        private readonly IRepository<Booth> _boothRepo;

        public BoothService(IRepository<Booth> repository)
        {
            _boothRepo = repository;
        }

        public Booth Book(string Username, string token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Counts the avalible booths.
        /// </summary>
        /// <returns>The avalible booths.</returns>
        public int CountAvalibleBooths()
        {
            return _boothRepo.GetAll().Where(x => x.Booker == null).Count();
        }

        /// <summary>
        /// Create the specified newBooth.
        /// </summary>
        /// <returns>The created booth</returns>
        /// <param name="newBooth">New booth.</param>
        public Booth Create(Booth newBooth)
        {
            newBooth.Id = 0;
            if (newBooth.Booker != null)
            {
                GetById(newBooth.Booker.Id);
            }
            return _boothRepo.Create(newBooth);
        }

        /// <summary>
        /// Delete the booth with this id.
        /// </summary>
        /// <returns>The deleted booth</returns>
        /// <param name="id">Identifier.</param>
        public Booth Delete(int id)
        {
            GetById(id);
            return _boothRepo.Delete(id);
        }

        /// <summary>
        /// Gets all booths.
        /// </summary>
        /// <returns>The booths</returns>
        public List<Booth> GetAll()
        {
            return _boothRepo.GetAll().ToList();
        }

        /// <summary>
        /// Gets the booth by id.
        /// </summary>
        /// <returns>The booth.</returns>
        /// <param name="id">Identifier.</param>
        public Booth GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be higher than 0");
            }
            var booth = _boothRepo.GetById(id);
            if (booth == null)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Booth with selected ID was not found.");

            }
            return booth;

        }

        /// <summary>
        /// Gets the users booking.
        /// </summary>
        /// <returns>The users booking.</returns>
        /// <param name="userId">User identifier.</param>
        public Booth GetUsersBooking(int userId)
        {
            return _boothRepo.GetAll().FirstOrDefault(b => b.Booker.Id == userId);
        }

        /// <summary>
        /// Update the specified booth.
        /// </summary>
        /// <returns>The updated booth</returns>
        /// <param name="updatedBooth">Updated booth.</param>
        public Booth Update(Booth updatedBooth)
        {
            GetById(updatedBooth.Id);
            return _boothRepo.Update(updatedBooth);
        }

        public int WaitingListPosition(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
