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
        readonly ILogService _logService;

        public BoothService(IRepository<User> userRepository,
                            IBoothRepository boothRepository,
                            IAuthenticationService authenticationService,
                            IWaitingListRepository waitinglistRepository,
                            ILogService logService)
        {
            _userRepository = userRepository;
            _boothRepository = boothRepository;
            _authService = authenticationService;
            _waitingListRepository = waitinglistRepository;
            _logService = logService;
        }

        #region Obsolete constructors
        [Obsolete("Constructor is used for tests, but should not be used elsewhere.")]
        public BoothService(IRepository<User> userRepository, 
                            IBoothRepository boothRepository,
                            IAuthenticationService authenticationService, 
                            IWaitingListRepository waitinglistRepository)
        {
            _userRepository = userRepository;
            _boothRepository = boothRepository;
            _authService = authenticationService;
            _waitingListRepository = waitinglistRepository;
        }
        #endregion

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

            // Check if the user exists
            if (user == null)
                throw new UserNotFoundException();

            // Checks if an empty booth exists
            var booth = _boothRepository.GetAllIncludeAll().FirstOrDefault(b => b.Booker == null);
            if (booth == null)
            {
                // Checks whether the user is already on a waiting list
                if (_waitingListRepository.GetAllIncludeAll().Any(w => w.Booker.Id == user.Id))
                    throw new AlreadyOnWaitingListException();
                
                // Puts the user on the waiting list
                else
                {
                    _waitingListRepository.Create(new WaitingListItem()
                    {
                        Date = DateTime.Now,
                        Booker = user
                    });

                    //LOG
                    _logService.Create($"{user?.Username} har fået en plads på ventelisten.", user);

                    throw new OnWaitingListException("Der var ikke flere tilgængelige stande men du er sat på venteliste");
                }
            }

            // Sets the user as booker on the waiting list
            booth.Booker = user;

            //LOG
            _logService.Create($"{user?.Username} har reserveret stand {booth?.Id} med tilfældig standreservering.", user);

            return _boothRepository.Update(booth);
        }

        /// <summary>
        /// Cancels a specific booth by removing the booker from it.
        /// </summary>
        /// <param name="boothId">The booth ID to be cancelled</param>
        /// <param name="token">The username of the user</param>
        public Booth CancelReservation(int boothId, string token)
        {
            var username = _authService.VerifyUserFromToken(token);

            var booth = GetByIdIncludeAll(boothId);

            // Checks if the booth exists
            if (booth == null)
            {
                throw new BoothNotFoundException();
            }
            // Checks that the booth has a booker
            if(booth.Booker == null)
            {
                throw new NotAllowedException("Cannot cancel a reservation, where a booth has no booker");
            }
            // Checks if the user is the one who has the booth booked
            if(booth.Booker.Username != username)
            {
                throw new NotAllowedException();
            }

            //LOG
            _logService.Create($"{booth?.Booker.Username} har annuleret deres stand nr. {booth?.Id}.", booth?.Booker);

            // Deletes the booker from the booth
            booth.Booker = null;

            // Finds the next user in the waiting list (sorted by date) and puts the on the newly cancelled booth
            var wli = _waitingListRepository.GetAllIncludeAll().FirstOrDefault(w => w.Date == _waitingListRepository.GetAll().Min(d => d.Date));
            if(wli != null)
            {
                booth.Booker = wli.Booker;
                _waitingListRepository.Delete(wli.Id);

                //LOG
                _logService.Create($"{wli?.Booker.Username} (Id: {wli?.Booker.Id}) har fået stand nr. {booth?.Id} efter at have været på ventelisten.", wli.Booker);
            }

            return _boothRepository.Update(booth);
        }

        /// <summary>
        /// Counts the avalible booths.
        /// </summary>
        /// <returns>The avalible booths.</returns>
        public int CountAvailableBooths()
        {
            return _boothRepository.GetAllIncludeAll().Where(x => x.Booker == null).Count();
        }

        /// <summary>
        /// Create the specified newBooth.
        /// </summary>
        /// <returns>The created booth</returns>
        /// <param name="newBooth">New booth.</param>
        public List<Booth> Create(int amount, Booth newBooth)
        {
            List<Booth> boothList = new List<Booth>();
            newBooth.Id = 0;

            // Checks if the new booths have a user set
            if (newBooth.Booker != null)
            {
                // Finds the user
                var user = _userRepository.GetById(newBooth.Booker.Id);

                // Checks if the user exists
                if (user == null)
                {
                    throw new UserNotFoundException();
                }
            }

            // Loops through all the booths that are to be made and creates the booth objects
            for (int i = 0; i < amount; i++)
            {
                Booth booth = new Booth()
                {
                    Id = newBooth.Id,
                    Booker = newBooth.Booker
                };
                boothList.Add(booth);
            }

            //LOG
            _logService.Create($"Der er blevet lavet {amount} nye stande.");

            return _boothRepository.Create(boothList);
        }

        /// <summary>
        /// Delete the booth with this id.
        /// </summary>
        /// <returns>The deleted booth</returns>
        /// <param name="id">Identifier.</param>
        public Booth Delete(int id)
        {
            // Checks if the booth exists, throws BoothNotFoundException if it doesn't
            GetById(id);

            //LOG
            _logService.Create($"Stand nr. {id} er blevet slettet.");

            return _boothRepository.Delete(id);
        }

        /// <summary>
        /// Cancels the position the waitinglist user is in
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WaitingListItem CancelWaitingPosition(string token)
        {
            WaitingListItem waitingListItem = _waitingListRepository.GetAllIncludeAll().FirstOrDefault(w => w.Booker.Username == _authService.VerifyUserFromToken(token));

            // Checks if the waiting list item exists
            if (waitingListItem == null)
            {
                throw new WaitingListItemNotFoundException();
            }
            // Checks if the item has a booker
            if (waitingListItem.Booker == null)
            {
                // Could be smart deleting the item since the item would be buggy if it doesn't have a booker
                throw new NotAllowedException("Det var ikke muligt at annullere din position i ventelisten");
            }

            //LOG
            _logService.Create($"{waitingListItem?.Booker.Username} har afmeldt sig fra ventelisten.", waitingListItem.Booker);

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
        public int GetWaitingListItemPosition(string token)
        {
            string username = _authService.VerifyUserFromToken(token);

            // Gets a int value that is equal to the user's position in the waiting list, sorted by date
            int? waitingListItemPosition = GetAllWaitingListItemsOrdered()
                .Select((s, i) => new { s, i })
                .Where(w => w.s.Booker?.Username == username)
                .Select(w => w.i + 1)
                .FirstOrDefault();

            // Checks if the user is on the waiting list
            if (waitingListItemPosition == null || waitingListItemPosition.Value == 0)
            {
                throw new NotOnWaitingListException();
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
            // Checks if the ID is valid
            if (id <= 0)
                throw new BoothNotFoundException(nameof(id) +  " ID must be higher than 0");
            
            var booth = _boothRepository.GetById(id);

            // Checks if the booth exists
            if (booth == null)
                throw new BoothNotFoundException(id);
                
            return booth;
        }
        
        /// <summary>
        /// Gets the booth by id.
        /// </summary>
        /// <returns>The booth.</returns>
        /// <param name="id">Identifier.</param>
        public Booth GetByIdIncludeAll(int id)
        {
            // Checks if the ID is valid
            if (id <= 0)
                throw new BoothNotFoundException(nameof(id) + "ID must be higher than 0");

            var booth = _boothRepository.GetByIdIncludeAll(id);

            // Checks if the booth exists
            if (booth == null)
                throw new BoothNotFoundException(id);

            return booth;
        }

        /// <summary>
        /// Gets the users booking.
        /// </summary>
        /// <returns>The users booking.</returns>
        /// <param name="token">User identifier.</param>
        public List<Booth> GetUsersBooking(string token)
        {
            var username = _authService.VerifyUserFromToken(token);
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);

            // Checks if the user exists
            if (user == null)
                throw new UserNotFoundException();
                
            var list = _boothRepository.GetAllIncludeAll().Where(b => b.Booker?.Id == user.Id).ToList();

            // Checks if the user has any bookings
            if (list.Count() == 0)
                throw new NoBookingsFoundException();

            return list;
        }

        /// <summary>
        /// Update the specified booth.
        /// </summary>
        /// <returns>The updated booth</returns>
        /// <param name="updatedBooth">Updated booth.</param>
        public Booth Update(Booth updatedBooth)
        {
            var booth = GetByIdIncludeAll(updatedBooth.Id);
             
            // Checks if the booth is supposed to be set as having no booth
            if (updatedBooth.Booker.Id == 0)
            {
                //LOG
                _logService.Create($"Stand nr. {updatedBooth?.Id} er blevet opdateret til ikke at have en standholder.");
                updatedBooth.Booker = null;
                return _boothRepository.Update(updatedBooth);
            }

            updatedBooth.Booker = _userRepository.GetById(updatedBooth.Booker.Id);

            // Checks if the user that is attempted to be set on the booth exists
            if (updatedBooth.Booker == null)
                throw new UserNotFoundException();

            // Runs if a booth is updated with a new user and there wasn't a user on the booth prior
            if (booth.Booker == null)
            {
                //LOG
                _logService.Create($"Stand nr. {updatedBooth?.Id} er blevet opdateret til at have standholder {updatedBooth?.Booker.Username}.", updatedBooth.Booker);
            }

            // Runs if a booth is updated with a ew user and there was a user on the booth prior
            else
            {
                //LOG
                _logService.Create($"Stand nr. {updatedBooth?.Id} er blevet opdateret til at have standholder {updatedBooth?.Booker.Username}. Gamle standholder: {booth.Booker?.Username} (Id: {booth?.Booker.Id})", updatedBooth.Booker);
            }

            return _boothRepository.Update(updatedBooth);
        }

        /// <summary>
        /// Gets all booths including the booker.
        /// </summary>
        public List<Booth> GetAllIncludeAll()
        {
            return _boothRepository.GetAllIncludeAll().Select(b => {
                // Removes Hash and Salt from the booker when displayed for frontend
                if (b.Booker != null)
                {
                    b.Booker.PasswordHash = null;
                    b.Booker.PasswordSalt = null;
                }
                return b;
            }).ToList();
        }

        /// <summary>
        /// Gets all booths that do not have a booker.
        /// </summary>
        public List<Booth> GetUnbookedBooths()
        {
            return _boothRepository.GetAllIncludeAll().Where(b => b.Booker == null).ToList();
        }

        /// <summary>
        /// Creates a new waiting list item with a user attached.
        /// </summary>
        /// <param name="token">The username of the user</param>
        public WaitingListItem AddToWaitingList(string token)
        {
            var username = _authService.VerifyUserFromToken(token);
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);

            // CHecks if the user exists
            if (user == null)
                throw new UserNotFoundException();

            // Date is set to the current time
            var waitingListItem = new WaitingListItem()
            {
                Id = 0,
                Booker = user,
                Date = DateTime.Now
            };

            // Removes Hash and Salt from the user for frontend
            var userWithoutPassword = _waitingListRepository.Create(waitingListItem);
            userWithoutPassword.Booker.PasswordHash = null;
            userWithoutPassword.Booker.PasswordSalt = null;

            //LOG
            _logService.Create($"{user?.Username} er blevet tilføjet på ventleisten. Ventelist id er {waitingListItem?.Id}", user);

            return userWithoutPassword;
        }

        /// <summary>
        /// Books a custom amount of booths with selected IDs for the user
        /// </summary>
        /// <param name="booths">The amount of booths to be booked</param>
        /// <param name="token">The username of the user</param>
        /// <returns></returns>
        public List<Booth> BookBoothsById(List<Booth> booths, string token)
        {
            // Checks if there are any booths being booked
             if (booths == null || booths.Count == 0)
                 throw new EmptyBookingException();
                
             var username = _authService.VerifyUserFromToken(token);
             var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);
             
            // Checks if the user exists
             if (user == null)
                 throw new UserNotFoundException();
                 
             // Adds the user as booker on all the selected booths
             booths.ForEach(b => {
                 var booth = _boothRepository.GetByIdIncludeAll(b.Id);
                 // Checks if the booth exists
                 if (booth == null)
                 {
                     throw new BoothNotFoundException();
                 }
                 // Checks that the booth doesn't already have a booker
                 else if (booth.Booker != null)
                 {
                     throw new AlreadyBookedException();
                 }
                 b.Booker = user;
             });

            //Required for LOG to log all booths being booked
            string boothIds = "";

            foreach (var boothId in booths)
            {
                boothIds += boothId.Id + ", ";
            }
            boothIds.Substring(boothIds.Length - 2);

            //LOG
            _logService.Create($"{user?.Username} (Id: {user?.Id}) har reserveret {booths?.Count} stande på id {boothIds}.", user);

            return _boothRepository.Update(booths);
        }
    }
}
