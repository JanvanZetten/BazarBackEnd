using Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface IImageURLService
    {
        ImageURL GetById(int id);
        List<ImageURL> GetAll();
        ImageURL Delete(int id);
        ImageURL Create(ImageURL imgurl);
        ImageURL Update(ImageURL imgurl);
    }
}
