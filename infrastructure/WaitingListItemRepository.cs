using Core.Domain;
using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace infrastructure
{
    public class WaitingListItemRepository : IWaitingListRepository
    {
        private readonly BazarContext _ctx;
        
        public WaitingListItemRepository(BazarContext ctx)
        {
            _ctx = ctx;
        }

        // Counts the amount of WaitingListItems in database.
        /// <summary>
        /// Counts the amount of WaitingListItem
        /// </summary>
        /// <returns>WaitingListItems count</returns>
        public int Count()
        {
            return _ctx.WaitingListItem.Count();
        }
        /// <summary>
        /// Creates a WaitingListItem
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Created WaitingListItem</returns>
        public WaitingListItem Create(WaitingListItem entity)
        {
            var savedUser = _ctx.WaitingListItem.Add(entity).Entity;
            _ctx.SaveChanges();
            return savedUser;
        }

        /// <summary>
        /// Deletes WaitingListItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted WaitingListItem</returns>
        public WaitingListItem Delete(int id)
        {
            var WaitingListItem = GetById(id);
            _ctx.WaitingListItem.Remove(WaitingListItem);
            _ctx.SaveChanges();
            return WaitingListItem;

        }
        /// <summary>
        /// Method to get all WaitingListItems
        /// </summary>
        /// <returns>All WaitingListItems</returns>
        public IEnumerable<WaitingListItem> GetAll()
        {
            return _ctx.WaitingListItem;
        }

        public IEnumerable<WaitingListItem> GetAllIncludeAll()
        {
            return _ctx.WaitingListItem.Include(w => w.Booker);
        }

        /// <summary>
        /// Get WaitingListItem with specific ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Specific WaitingListItem</returns>
        public WaitingListItem GetById(int id)
        {
            return _ctx.WaitingListItem.FirstOrDefault(w => w.Id == id);
        }
        /// <summary>
        /// Updating WaitingListItem, using attached to also update references.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Updated WaitingListItem</returns>
        public WaitingListItem Update(WaitingListItem entity)
        {
            _ctx.Attach(entity).State = EntityState.Modified;
            _ctx.Entry(entity).Reference(w => w.Booker).IsModified = true;
            _ctx.SaveChanges();
            return entity;
        }

    }
}
