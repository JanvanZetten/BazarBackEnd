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
    public class CreateURLTest
    {
        private Mock<IImageURLRepository> mockURLRepository = new Mock<IImageURLRepository>();
        private readonly IImageURLService _urlService;
        private Dictionary<int, ImageURL> urlDictionary = new Dictionary<int, ImageURL>();
        private int nextId = 1;

        public CreateURLTest()
        {
            mockURLRepository.Setup(x => x.Create(It.IsAny<ImageURL>())).Returns<ImageURL>((u) =>
            {
                u.Id = nextId++;
                urlDictionary.Add(u.Id, u);
                return urlDictionary[u.Id];
            });
            
            _urlService = new ImageURLService(mockURLRepository.Object);
        }

        [InlineData("picture.png")]
        [InlineData("picture.jpg")]
        [InlineData("picture.jpeg")]
        [InlineData("picture.gif")]
        [InlineData("picture.PNG")]
        [InlineData("picture.JPeG")]
        [Theory] 
        public void AssertCreateWithValidURL(string url)
        {
            ImageURL urltmp = new ImageURL()
            {
                URL = url
            };
            var result = _urlService.Create(urltmp);

            Assert.True(urlDictionary.Values.Any(u => u.URL == urltmp.URL));
            Assert.True(result.URL == url);
        }

        [InlineData("picturepng")]
        [InlineData("picture.exe")]
        [InlineData("picture.bmp")]
        [InlineData("picture.mp3")]
        [Theory]
        public void AssertCreateWithInvalidURLThrowsException(string url)
        {
            ImageURL urltmp = new ImageURL()
            {
                URL = url
            };

            Assert.Throws<IncompatibleFileTypeException>(() =>
            {
                var result = _urlService.Create(urltmp);
            });
        }

        [Fact]
        public void AssertThrowsNullreferenceException()
        {
            ImageURL urltmp = new ImageURL()
            {
                URL = null
            };

            Assert.Throws<InputNotValidException>(() =>
            {
                var result = _urlService.Create(urltmp);
            });

            Assert.Throws<InputNotValidException>(() =>
            {
                var result = _urlService.Create(null);
            });
        }
    }
}
