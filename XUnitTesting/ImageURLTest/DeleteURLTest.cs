using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace XUnitTesting.ImageURLTest
{
    public class DeleteURLTest
    {
        private Mock<IImageURLRepository> mockURLRepository = new Mock<IImageURLRepository>();
        private readonly IImageURLService _urlService;
        private Dictionary<int, ImageURL> urlDictionary = new Dictionary<int, ImageURL>();

        private readonly ImageURL url1 = new ImageURL()
        {
            Id = 1,
            URL = "hej.png"
        };
        private readonly ImageURL url2 = new ImageURL()
        {
            Id = 2,
            URL = "mojn.gif"
        };

        public DeleteURLTest()
        {

            mockURLRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (!urlDictionary.ContainsKey(id))
                    return null;
                var url = urlDictionary[id];
                urlDictionary.Remove(id);
                return url;
            });

            _urlService = new ImageURLService(mockURLRepository.Object);
        }

        [Fact]
        public void DeleteValidInput()
        {
            urlDictionary.Clear();
            urlDictionary.Add(url1.Id, url1);
            urlDictionary.Add(url2.Id, url2);

            var deletedURL = _urlService.Delete(url1.Id);

            Assert.True(!urlDictionary.Values.Any(u => u.Id == url1.Id));
            Assert.Equal(url1.URL, deletedURL.URL);
            Assert.Equal(url1.Id, deletedURL.Id);
        }

        [Fact]
        public void DeleteInvalidInput()
        {
            urlDictionary.Clear();
            urlDictionary.Add(url1.Id, url1);
            urlDictionary.Add(url2.Id, url2);

            Assert.Throws<ImageURLNotFoundException>(() =>
            {
                _urlService.Delete(3);
            });
            Assert.Equal(2, urlDictionary.Count);
        }
    }
}
