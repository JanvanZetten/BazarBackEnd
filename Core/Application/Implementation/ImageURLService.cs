using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;

namespace Core.Application.Implementation
{
    public class ImageURLService : IImageURLService
    {
        private IImageURLRepository _urlRepo;

        public ImageURLService(IImageURLRepository urlRepo)
        {
            _urlRepo = urlRepo;
        }

        public ImageURL Create(ImageURL imgurl)
        {
            imgurl.Id = 0;
            if (imgurl == null || imgurl.URL == null)
                throw new InputNotValidException("URL kan ikke være tom.");
            string ext = Path.GetExtension(imgurl.URL).ToLower();
            if (ext == null || (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif"))
                throw new IncompatibleFileTypeException();

            return _urlRepo.Create(imgurl);
        }

        public ImageURL Delete(int id)
        {
            var url = _urlRepo.Delete(id);
            if (url == null)
                throw new ImageURLNotFoundException();
            return url;
        }

        public List<ImageURL> GetAll()
        {
            return _urlRepo.GetAll().ToList();
        }

        public ImageURL GetById(int id)
        {
            ImageURL url = _urlRepo.GetById(id);

            if (url == null)
                throw new ImageURLNotFoundException();

            return url;
        }

        public ImageURL Update(ImageURL imgurl)
        {
            if (imgurl == null || imgurl.URL == null)
                throw new InputNotValidException("URL kan ikke være tom.");
            string ext = Path.GetExtension(imgurl.URL).ToLower();
            if (ext == null || (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif"))
                throw new IncompatibleFileTypeException();

            ImageURL url = _urlRepo.GetById(imgurl.Id);
            if (url == null)
                throw new ImageURLNotFoundException();

            return _urlRepo.Update(imgurl);
        }
    }
}
