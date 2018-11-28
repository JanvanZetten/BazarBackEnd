using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public interface IRepository<T>
    {
        T Create(T entity);
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Update(T entity);
        T Delete(int id);
        int Count();
    }
}
