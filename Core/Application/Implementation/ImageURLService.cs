﻿using System;
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
            if (imgurl == null || imgurl.URL == null)
                throw new ImageURLNotFoundException();
            string ext = Path.GetExtension(imgurl.URL).ToLower();
            if (ext == null || (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif"))
                throw new IncompatibleFileTypeException();

            return _urlRepo.Create(imgurl);
        }

        public ImageURL Delete(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
