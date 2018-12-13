using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface ILogService
    {
        Log Create(Log log);
        Log Delete(int id);
        Log GetById(int id);
        List<Log> GetAll();
    }
}
