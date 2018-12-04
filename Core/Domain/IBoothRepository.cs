using System;
using System.Collections.Generic;
using Core.Entity;

namespace Core.Domain
{
    public interface IBoothRepository: IRepository<Booth>
    {
        IEnumerable<Booth> GetAllIncludeAll();
    }
}
