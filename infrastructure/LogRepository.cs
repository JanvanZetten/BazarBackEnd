using Core.Domain;
using Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace infrastructure
{
    public class LogRepository : ILogRepository
    {


        private BazarContext _ctx;

        public LogRepository(BazarContext ctx)
        {
            _ctx = ctx;
        }

        public int Count()
        {
            return _ctx.Log.Count();
        }

        public Log Create(Log entity)
        {
            _ctx.Attach(entity).State = EntityState.Added;
            _ctx.SaveChanges();
            return entity;
        }

        public Log Delete(int id)
        {
            var log = GetById(id);
            _ctx.Log.Remove(log);
            _ctx.SaveChanges();
            return log;
        }

        public IEnumerable<Log> GetAll()
        {
            return _ctx.Log;
        }

        public IEnumerable<Log> GetAllIncludeAll()
        {
            return _ctx.Log.Include(l => l.User);
        }

        public Log GetById(int id)
        {
            Log log = _ctx.Log.FirstOrDefault(l => l.Id == id);
            _ctx.Entry(log).State = EntityState.Detached;
            return log;
        }

        public Log GetByIdIncludeAll(int id)
        {
            var log = _ctx.Log.Include(l => l.Id).FirstOrDefault(l => l.Id == id);
            _ctx.Entry(log).State = EntityState.Detached;
            return log;
        }

        public Log Update(Log entity)
        {
            var oldLog = GetById(entity.Id);
            if (oldLog == null)
            {
                return null;
            }
            _ctx.Attach(entity).State = EntityState.Modified;
            _ctx.Entry(entity).Reference(l => l.User).IsModified = true;
            _ctx.SaveChanges();
            return entity;
        }
    }
}

