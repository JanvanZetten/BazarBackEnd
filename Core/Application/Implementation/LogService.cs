using System;
using System.Collections.Generic;
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

        public Log Create(Log log)
        {
            if(log == null)
            {
                throw new NullReferenceException("Log må ikke være tom");
            }
            if(log.Message == null)
            {
                throw new InputNotValidException("Beskeden må ikke være tom");
            }
            if(log.User != null)
            {
                var user = _userRepository.GetById(log.User.Id);
                if(user == null)
                {
                    throw new UserNotFoundException();
                }
            }
            log.Id = 0;
            log.Date = DateTime.Now;
            return _logRepository.Create(log);
        }

        public Log Delete(int id)
        {
            var log = GetById(id);
            return _logRepository.Delete(id);
        }

        public List<Log> GetAll()
        {
            throw new NotImplementedException();
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
