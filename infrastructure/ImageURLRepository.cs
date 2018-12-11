using Core.Domain;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace infrastructure
{
    public class ImageURLRepository : IImageURLRepository
    {
        private BazarContext _ctx;

        public ImageURLRepository(BazarContext ctx)
        {
            _ctx = ctx;
        }

        public int Count()
        {
            return _ctx.ImageURL.Count();
        }

        public ImageURL Create(ImageURL entity)
        {
            _ctx.ImageURL.Add(entity);
            _ctx.SaveChanges();
            return entity;
        }

        public ImageURL Delete(int id)
        {
            var url = GetById(id);
            _ctx.ImageURL.Remove(url);
            _ctx.SaveChanges();
            return url;
        }

        public IEnumerable<ImageURL> GetAll()
        {
            return _ctx.ImageURL;
        }

        public ImageURL GetById(int id)
        {
            var url = _ctx.ImageURL.FirstOrDefault(u => u.Id == id);
            return url;
        }

        public ImageURL Update(ImageURL entity)
        {
            var oldurl = GetById(entity.Id);
            if (oldurl == null)
                return null;

            oldurl.URL = entity.URL;

            var newurl = _ctx.ImageURL.Update(oldurl).Entity;
            _ctx.SaveChanges();
            return newurl;
        }
    }
}
