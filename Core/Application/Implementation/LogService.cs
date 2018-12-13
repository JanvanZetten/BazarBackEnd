using System;
using System.Collections.Generic;
using System.Text;
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
            throw new NotImplementedException();
        }

        public Log Delete(Log log)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Log> GetAll()
        {
            throw new NotImplementedException();
        }

        public Log GetById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
