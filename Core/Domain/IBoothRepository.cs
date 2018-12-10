using System;
using System.Collections.Generic;
using Core.Entity;

namespace Core.Domain
{
    public interface IBoothRepository: IRepository<Booth>
    {
        IEnumerable<Booth> GetAllIncludeAll();
        Booth GetByIdIncludeAll(int id);
        List<Booth> Create(List<Booth> boothList);
    }
}
