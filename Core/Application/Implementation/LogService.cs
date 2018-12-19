using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;

namespace Core.Application.Implementation
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;

        public LogService(ILogRepository logRepository, IUserRepository userRepository)
        {
            this._logRepository = logRepository;
            this._userRepository = userRepository;
        }

        /// <summary>
        /// Creates a new log entry. Used when logging an action.
        /// </summary>
        /// <param name="message">The message that will be shown as the logged action.</param>
        /// <param name="user">The user who caused the log. Does not require a user if an admin caused the log.</param>
        public Log Create(String message, User user = null)
        {
            // Checks if a message is sent over
            if(message == null)
            {
                throw new InputNotValidException("Beskeden må ikke være tom");
            }

            // Runs if a user was sent over
            if(user != null)
            {
                var userFromLog = _userRepository.GetById(user.Id);

                // Checks if the user exists
                if(userFromLog == null)
                {
                    throw new UserNotFoundException();
                }
            }

            // Date is set to current time and a log is created
            Log log = new Log() { Id = 0, Message = message, User = user, Date = DateTime.Now };

            return _logRepository.Create(log);
        }

        /// <summary>
        /// Deletes a log entry with a given ID
        /// </summary>
        /// <param name="id">ID of the log entry</param>
        public Log Delete(int id)
        {
            // Checks if the log entry exists, exception is thrown in this method
            GetById(id);

            return _logRepository.Delete(id);
        }

        /// <summary>
        /// Returns all log entries
        /// </summary>
        public List<Log> GetAll()
        {
            return _logRepository.GetAllIncludeAll().Select(l => {
                // Removes Hash and Salt from the user for frontend
                if(l.User != null)
                {
                    l.User.PasswordHash = null;
                    l.User.PasswordSalt = null;
                }
                return l;
            }).ToList();
        }

        /// <summary>
        /// Returns a log entry with the given ID
        /// </summary>
        /// <param name="id">The ID of the log entry</param>
        public Log GetById(int id)
        {
            var log = _logRepository.GetByIdIncludeAll(id);

            // Checks if the log entry exists
            if(log == null)
            {
                throw new LogNotFoundException();
            }
            return log;
        }
    }
}
