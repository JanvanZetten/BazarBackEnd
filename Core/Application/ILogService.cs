using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface ILogService
    {
        Log Create(Log log);
        Log Delete(Log log);
        Log GetById(int Id);
        IEnumerable<Log> GetAll();
    }
}
