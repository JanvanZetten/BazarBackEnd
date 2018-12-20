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

        /// <summary>
        /// Creates a new field in the database for a new image link
        /// </summary>
        /// <param name="imgurl">The given image URL</param>
        public ImageURL Create(ImageURL imgurl)
        {
            // Checks if the URL contains something
            if (imgurl == null || imgurl.URL == null)
            {
                throw new InputNotValidException("URL kan ikke være tom.");
            }

            imgurl.Id = 0;
            string ext = Path.GetExtension(imgurl.URL).ToLower();

            // Checks if the URL leads to one of the following file types, (&& ext != ".XXX") to add a new filetype
            // Remember to change in UPDATE method as well
            if (ext == null || (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif"))
            {
                throw new IncompatibleFileTypeException();
            }

            return _urlRepo.Create(imgurl);
        }

        /// <summary>
        /// Deletes a URL field in the database
        /// </summary>
        /// <param name="id">The ID of the field</param>
        public ImageURL Delete(int id)
        {
            var url = _urlRepo.Delete(id);

            // Checks if the URL exists
            if (url == null)
                throw new ImageURLNotFoundException();

            return url;
        }

        /// <summary>
        /// Returns all URLs in the database
        /// </summary>
        public List<ImageURL> GetAll()
        {
            return _urlRepo.GetAll().ToList();
        }

        /// <summary>
        /// Returns a specific URL in the database
        /// </summary>
        /// <param name="id">ID of the URL</param>
        /// <returns></returns>
        public ImageURL GetById(int id)
        {
            ImageURL url = _urlRepo.GetById(id);

            // Checks if the URL exists
            if (url == null)
                throw new ImageURLNotFoundException();

            return url;
        }

        /// <summary>
        /// Updates an URL in the database
        /// </summary>
        /// <param name="imgurl">The new URL with the same ID as the one to be updated</param>
        public ImageURL Update(ImageURL imgurl)
        {
            // Checks if the URL contains something
            if (imgurl == null || imgurl.URL == null)
                throw new InputNotValidException("URL kan ikke være tom.");

            string ext = Path.GetExtension(imgurl.URL).ToLower();

            // Checks if the URL leads to one of the following file types, (&& ext != ".XXX") to add a new filetype
            // Remember to change in CREATE method as well
            if (ext == null || (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif"))
                throw new IncompatibleFileTypeException();

            ImageURL url = _urlRepo.GetById(imgurl.Id);

            // Checks if the URL exists on the given ID
            if (url == null)
                throw new ImageURLNotFoundException();

            //LOG
            _logService.Create($"Billedet med id: {url.Id} blev skiftet fra {url.URL} til {imgurl.URL}");

            return _urlRepo.Update(imgurl);
        }
    }
}
