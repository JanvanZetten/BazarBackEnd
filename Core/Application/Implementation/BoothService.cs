using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;

namespace Core.Application.Implementation
{
    public class BoothService : IBoothService
    {
        readonly IRepository<User> _userRepository;
        readonly IBoothRepository _boothRepository;
        readonly IAuthenticationService _authService;
        readonly IWaitingListRepository _waitingListRepository;

        public BoothService(IRepository<User> userRepository, IBoothRepository boothRepository,
         IAuthenticationService authenticationService, IWaitingListRepository waitinglistRepository)
        {
            _userRepository = userRepository;
            _boothRepository = boothRepository;
            _authService = authenticationService;
            _waitingListRepository = waitinglistRepository;
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
            {
                if (_waitingListRepository.GetAll().Any(w => w.Booker.Id == user.Id))
                    throw new NotSupportedException("Du er allerede på ventelisten.");
                else
                {
                    _waitingListRepository.Create(new WaitingListItem()
                    {
                        Date = DateTime.Now,
                        Booker = user
                    });

                    throw new OnWaitingListException("Der var ikke flere tilgængelige stande men du er sat på venteliste");
                }
            }

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

            var wli = _waitingListRepository.GetAll().FirstOrDefault(w => w.Date == _waitingListRepository.GetAll().Min(d => d.Date));
            if(wli != null)
            {
                booth.Booker = wli.Booker;
                _waitingListRepository.Delete(wli.Id);
            }
            
            return _boothRepository.Update(booth);

        }

        /// <summary>
        /// Counts the avalible booths.
        /// </summary>
        /// <returns>The avalible booths.</returns>
        public int CountAvailableBooths()
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
        /// Cancels the position the waitinglist user is in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WaitingListItem CancelWaitingPosition(int waitingId, string token)
        {
            var username = _authService.VerifyUserFromToken(token);
            var waitingListItem = _waitingListRepository.GetById(waitingId);
            if (waitingListItem == null)
            {
                throw new ArgumentOutOfRangeException("Did not find waiting list item");
            }
            if (waitingListItem.Booker == null)
            {
                throw new ArgumentException("Cannot cancel position in waiting list");
            }
            if (username == null)
            {
                throw new ArgumentException("Not a valid user");
            }
            if (waitingListItem.Booker.Username != username)
            {
                throw new ArgumentException("Not a valid user");
            }

            return _waitingListRepository.Delete(waitingListItem.Id);

        }
        /// <summary>
        /// Gets all waiting list items
        /// </summary>
        /// <returns>The list of all waiting items</returns>
        private IEnumerable<WaitingListItem> GetAllWaitingListItemsOrdered()
        {
            return _waitingListRepository.GetAllIncludeAll().OrderBy(w => w.Date);
        }

        /// <summary>
        /// Gets the position the user is in waiting list
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Position in waiting list</returns>
        int IBoothService.GetWaitingListItemPosition(string token)
        {
            string username = _authService.VerifyUserFromToken(token);

            int? waitingListItemPosition = GetAllWaitingListItemsOrdered()
                .Select((s, i) => new { s, i })
                .Where(w => w.s.Booker?.Username == username)
                .Select(w => w.i + 1)
                .FirstOrDefault();

            if (waitingListItemPosition == 0 && waitingListItemPosition == null)
            {
                throw new ArgumentOutOfRangeException("Invalid user, user is not in waiting list");
            }
            return waitingListItemPosition.Value;
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
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be higher than 0");
            
            var booth = _boothRepository.GetById(id);
            if (booth == null)
                throw new ArgumentOutOfRangeException(nameof(id), "Booth with selected ID was not found.");
                
            return booth;

        }

        /// <summary>
        /// Gets the users booking.
        /// </summary>
        /// <returns>The users booking.</returns>
        /// <param name="token">User identifier.</param>
        public Booth GetUsersBooking(string token)
        {
            var username = _authService.VerifyUserFromToken(token);
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);

            if (user == null)
                throw new ArgumentOutOfRangeException("Could not find the specified user.");
            
            return _boothRepository.GetAll().FirstOrDefault(b => b.Booker.Id == user.Id);
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

    }
}
