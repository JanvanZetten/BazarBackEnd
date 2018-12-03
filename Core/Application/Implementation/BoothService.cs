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

        /// <summary>
        /// Books a booth for the user found in the token. 
        /// If the token is invalid; the user is not in the repository; or no booth is available an exception is thrown.
        /// </summary>
        /// <param name="token">JWT Token.</param>
        /// <returns>Booth which was booked.</returns>
        public Booth Book(string token)
        {
            var username = _authService.VerifyUserFromToken(token);
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);

            if (user == null)
                throw new ArgumentOutOfRangeException("Could not find the specified user.");
            
            var booth = _boothRepository.GetAll().FirstOrDefault(b => b.Booker == null);
            if (booth == null)
                throw new InvalidOperationException("No booths available.");

            booth.Booker = user;
            return Update(booth);
        }

        public Booth CancelReservation(int boothId, string token)
        {
            var username = _authService.VerifyUserFromToken(token);
            var booth = GetById(boothId);
            if(booth == null)
            {
                throw new ArgumentOutOfRangeException("Did not find booth");
            }
            if(booth.Booker == null)
            {
                throw new ArgumentException("Cannot cancel a reservation, where a booth has no booker");
            }
            if(username == null)
            {
                throw new ArgumentException("Not a valid user");
            }
            if(booth.Booker.Username != username)
            {
                throw new ArgumentException("Not a valid user");
            }
            booth.Booker = null;
            
            return _boothRepository.Update(booth);

        }

        /// <summary>
        /// Counts the avalible booths.
        /// </summary>
        /// <returns>The avalible booths.</returns>
        public int CountAvalibleBooths()
        {
            return _boothRepository.GetAll().Where(x => x.Booker == null).Count();
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
            return _boothRepository.Create(newBooth);
        }

        /// <summary>
        /// Delete the booth with this id.
        /// </summary>
        /// <returns>The deleted booth</returns>
        /// <param name="id">Identifier.</param>
        public Booth Delete(int id)
        {
            GetById(id);
            return _boothRepository.Delete(id);
        }

        /// <summary>
        /// Gets all booths.
        /// </summary>
        /// <returns>The booths</returns>
        public List<Booth> GetAll()
        {
            return _boothRepository.GetAll().ToList();
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
            var booth = _boothRepository.GetById(id);
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
            return _boothRepository.GetAll().FirstOrDefault(b => b.Booker.Id == userId);
        }

        /// <summary>
        /// Update the specified booth.
        /// </summary>
        /// <returns>The updated booth</returns>
        /// <param name="updatedBooth">Updated booth.</param>
        public Booth Update(Booth updatedBooth)
        {
            GetById(updatedBooth.Id);
            return _boothRepository.Update(updatedBooth);
        }

        public int WaitingListPosition(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
