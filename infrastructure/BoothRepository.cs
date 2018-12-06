using Core.Domain;
using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace infrastructure
{
    public class BoothRepository : IBoothRepository
    {
        private readonly BazarContext _ctx;

        public BoothRepository(BazarContext ctx)
        {
            _ctx = ctx;
        }
        // Counts the amount of booths in database.
        /// <summary>
        /// Counts the amount of booth
        /// </summary>
        /// <returns>booths count</returns>
        public int Count()
        {
            return _ctx.Booth.Count();
        }
        /// <summary>
        /// Creates a booth
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Created booth</returns>
        public Booth Create(Booth entity)
        {
            var savedUser =_ctx.Booth.Add(entity).Entity;
            _ctx.SaveChanges();
            return savedUser;
        }
        /// <summary>
        /// Deletes booth
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted booth</returns>
        public Booth Delete(int id)
        {
            var booth = GetById(id);
            _ctx.Booth.Remove(booth);
            _ctx.SaveChanges();
            return booth;
            
        }
        /// <summary>
        /// Method to get all booths
        /// </summary>
        /// <returns>All booths</returns>
        public IEnumerable<Booth> GetAll()
        {
            return _ctx.Booth;
        }

        public IEnumerable<Booth> GetAllIncludeAll()
        {
            return _ctx.Booth.Include(b => b.Booker);
        }

        /// <summary>
        /// Get booth with specific ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Specific booth</returns>
        public Booth GetById(int id)
        {
            Booth booth = _ctx.Booth.FirstOrDefault(b => b.Id == id);
            _ctx.Entry(booth).State = EntityState.Detached;
            return booth;
        }

        public Booth GetByIdIncludeAll(int id)
        {
            Booth booth = _ctx.Booth.Include(b => b.Booker).FirstOrDefault(b => b.Id == id);
            _ctx.Entry(booth).State = EntityState.Detached;
            return booth;
        }

        /// <summary>
        /// Updating booth, using attached to also update references.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Updated booth</returns>
        public Booth Update(Booth entity)
        {
            _ctx.Attach(entity).State = EntityState.Modified;
            _ctx.Entry(entity).Reference(b => b.Booker).IsModified = true;
            _ctx.SaveChanges();
            return entity;
        }
    }
}
