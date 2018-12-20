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
        private Mock<ILogService> mockLogService = new Mock<ILogService>();
        private readonly IImageURLService _urlService;
        private Dictionary<int, ImageURL> urlDictionary = new Dictionary<int, ImageURL>();
        private int nextId = 1;

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public CreateURLTest()
        {
            mockURLRepository.Setup(x => x.Create(It.IsAny<ImageURL>())).Returns<ImageURL>((u) =>
            {
                u.Id = nextId++;
                urlDictionary.Add(u.Id, u);
                return urlDictionary[u.Id];
            });
            
            _urlService = new ImageURLService(mockURLRepository.Object, mockLogService.Object);
        }

        [InlineData("picture.png")]
        [InlineData("picture.jpg")]
        [InlineData("picture.jpeg")]
        [InlineData("picture.gif")]
        [InlineData("picture.PNG")]
        [InlineData("picture.JPeG")]
        /// <summary>
        /// Test creating valid new URLs
        /// </summary>
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
        /// <summary>
        /// Test creating invalid new URLs and throw exception
        /// </summary>
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

        /// <summary>
        /// Test to throw exception when URL is empty
        /// </summary>
        [Fact]
        public void AssertThrowsExceptionWhenURLIsNull()
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

        /// <summary>
        /// Test to make sure ID is automatically set
        /// </summary>
        [Fact]
        public void CreateImageUrlWithIdSetToZero()
        {
            mockURLRepository.Setup(x => x.Create(It.IsAny<ImageURL>())).Returns<ImageURL>((u) =>
            {
                urlDictionary.Add(u.Id, u);
                return urlDictionary[u.Id];
            });

            var result = _urlService.Create(new ImageURL() { Id = 200, URL="dsakdksakdo.gif" });

            Assert.True(result.Id == 0);
        }
    }
}
