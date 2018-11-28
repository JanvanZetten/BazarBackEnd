using Core.Domain;
using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace infrastructure
{
    public class BoothRepository : IRepository<Booth>
    {
        private readonly BazarContext _ctx;

        public BoothRepository(BazarContext ctx)
        {
            _ctx = ctx;
        }

        public int Count()
        {
            return _ctx.Booth.Count();
        }

        public Booth Create(Booth entity)
        {
            // Making variable for when we have to hash password etc. 
            var savedUser =_ctx.Booth.Add(entity).Entity;
            _ctx.SaveChanges();
            return savedUser;
        }

        public Booth Delete(int id)
        {
            var booth = GetById(id);
            if(booth != null)
            {
                _ctx.Booth.Remove(booth);
                _ctx.SaveChanges();
            }
            else
            {
                throw new NullReferenceException("Booth was not found, use a valid Booth ID");
            }
            return booth;
            
        }

        public IEnumerable<Booth> GetAll()
        {
            return _ctx.Booth;
        }

        public Booth GetById(int id)
        {
            return _ctx.Booth.FirstOrDefault(b => b.Id == id);
        }

        public Booth Update(Booth entity)
        {
            _ctx.Attach(entity).State = EntityState.Modified;
            _ctx.Entry(entity).Reference(b => b.Booker).IsModified = true;
            _ctx.SaveChanges();
            return entity;
        }
    }
}
