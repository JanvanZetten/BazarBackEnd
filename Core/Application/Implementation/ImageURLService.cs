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
        private readonly IImageURLRepository _urlRepo;
        private readonly ILogService _logService;

        public ImageURLService(IImageURLRepository urlRepo, ILogService logService)
        {
            _urlRepo = urlRepo;
            _logService = logService;
        }

        #region Obsolete Consructors

        [Obsolete("Use the constructor with the ILogService.")]
        public ImageURLService(IImageURLRepository urlRepo)
        {
            _urlRepo = urlRepo;
        }

        #endregion


        public ImageURL Create(ImageURL imgurl)
        {

            if (imgurl == null || imgurl.URL == null)
            {
                throw new InputNotValidException("URL kan ikke være tom.");
            }
            imgurl.Id = 0;
            string ext = Path.GetExtension(imgurl.URL).ToLower();
            if (ext == null || (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif"))
            {
                throw new IncompatibleFileTypeException();
            }

            var imageURLReturned = _urlRepo.Create(imgurl);

            return imageURLReturned;
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


            var returnedURL = _urlRepo.Update(imgurl);

            //LOG
            _logService.Create(new Log()
            {
                Message = $"Billedet med id: {url.Id} blev skiftet fra {url.URL} til {imgurl.URL}"
            });


            return returnedURL;
        }
    }
}
