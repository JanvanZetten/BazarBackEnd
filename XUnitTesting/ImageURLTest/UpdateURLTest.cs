using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTesting.ImageURLTest
{
    public class UpdateURLTest
    {
        private Mock<IImageURLRepository> mockURLRepository = new Mock<IImageURLRepository>();
        private Mock<ILogService> mockLogService = new Mock<ILogService>();
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

        public UpdateURLTest()
        {
            urlDictionary.Add(url1.Id, url1);
            urlDictionary.Add(url2.Id, url2);

            mockURLRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (urlDictionary.ContainsKey(id))
                    return urlDictionary[id];
                else
                    return null;
            });

            mockURLRepository.Setup(x => x.Update(It.IsAny<ImageURL>())).Returns<ImageURL>((u) =>
            {
                if (u == null)
                    return null;

                if (urlDictionary.ContainsKey(u.Id))
                {
                    urlDictionary[u.Id] = u;
                    return urlDictionary[u.Id];
                }
                else
                {
                    return null;
                }
            });

            _urlService = new ImageURLService(mockURLRepository.Object, mockLogService.Object);
        }

        [Fact]
        public void AssertUpdateURLValid()
        {
            var url = new ImageURL()
            {
                Id = 1,
                URL = "gutenmorgen.png"
            };

            var result = _urlService.Update(url);
            Assert.Equal(url.URL, result.URL);
            Assert.Equal(url.URL, urlDictionary[1].URL);
            Assert.True(urlDictionary.Count == 2);
        }

        [Fact]
        public void AssertThrowsWhenURLIsNull()
        {
            var url = new ImageURL()
            {
                Id = 1
            };

            Assert.Throws<InputNotValidException>(() =>
            {
                _urlService.Update(url);
            });

            ImageURL urlNull = null;

            Assert.Throws<InputNotValidException>(() =>
            {
                _urlService.Update(urlNull);
            });
        }

        [InlineData(3)]
        [InlineData(0)]
        [Theory]
        public void AssertThrowsWhenInvalidId(int id)
        {
            var url = new ImageURL()
            {
                Id = id,
                URL = "The FitnessGram™ Pacer Test is a multistage aerobic capacity test that progressively gets more difficult as it continues.png"
            };

            Assert.Throws<ImageURLNotFoundException>(() =>
            {
                _urlService.Update(url);
            });
        }

        [InlineData("picturepng")]
        [InlineData("picture.exe")]
        [InlineData("picture.bmp")]
        [InlineData("picture.mp3")]
        [Theory]
        public void AssertThrowsWhenInvalidFiletype(string url)
        {
            ImageURL urltmp = new ImageURL()
            {
                Id = 1,
                URL = url
            };

            Assert.Throws<IncompatibleFileTypeException>(() =>
            {
                var result = _urlService.Update(urltmp);
            });
        }

        [Fact]
        public void LogImageURLUpdate()
        {
            String newImageURL = "RandomNewImageURL.png";
            String oldImageURL = "RandomOldImageURL.png";
            int id = 1;

            mockURLRepository.Setup(mur => mur.GetById(It.IsAny<int>())).Returns(() => new ImageURL() { Id = id, URL = oldImageURL });

            _urlService.Update(new ImageURL() {Id = id , URL = newImageURL});

            mockLogService.Verify(mls => mls.Create(It.Is<Log>(l => l.Message.Equals($"Billedet med id: {id} blev skiftet fra {oldImageURL} til {newImageURL}"))));

        }
    }
}
