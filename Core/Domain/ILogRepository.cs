using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public interface ILogRepository:IRepository<Log>
    {
        IEnumerable<Log> GetAllIncludeAll();
        Log GetByIdIncludeAll(int id);
    }
}
