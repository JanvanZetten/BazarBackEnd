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

        public Log Create(String message, User user = null)
        {
            if(message == null)
            {
                throw new InputNotValidException("Beskeden må ikke være tom");
            }
            if(user != null)
            {
                var userFromLog = _userRepository.GetById(user.Id);
                if(userFromLog == null)
                {
                    throw new UserNotFoundException();
                }
            }
            Log log = new Log() { Id = 0, Message = message, User = user, Date = DateTime.Now };

            return _logRepository.Create(log);
        }

        public Log Delete(int id)
        {
            var log = GetById(id);
            return _logRepository.Delete(id);
        }

        public List<Log> GetAll()
        {
            return _logRepository.GetAllIncludeAll().Select(l => {
                if(l.User != null)
                {
                    l.User.PasswordHash = null;
                    l.User.PasswordSalt = null;
                }
                return l;
            }).ToList();
        }

        public Log GetById(int id)
        {
            var log = _logRepository.GetByIdIncludeAll(id);
            if(log == null)
            {
                throw new LogNotFoundException();
            }
            return log;
        }
    }
}
