using Core.Application;
using Core.Application.Implementation;
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
    public class GetAllURLTest
    {
        private Mock<IImageURLRepository> mockURLRepository = new Mock<IImageURLRepository>();
        private Dictionary<int, ImageURL> urlDictionary = new Dictionary<int, ImageURL>();

        private readonly IImageURLService _urlService;

        private readonly ImageURL url1 = new ImageURL()
        {
            Id = 1,
            URL = "hej"
        };
        private readonly ImageURL url2 = new ImageURL()
        {
            Id = 2,
            URL = "mojn"
        };

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public GetAllURLTest()
        {
            urlDictionary.Add(url1.Id, url1);
            urlDictionary.Add(url2.Id, url2);

            mockURLRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return urlDictionary.Values;
            });

            _urlService = new ImageURLService(mockURLRepository.Object);
        }

        /// <summary>
        /// Test to return all correct URLs 
        /// </summary>
        [Fact]
        public void AssertGetAllCorrectURL()
        {
            Assert.True(_urlService.GetAll().Count == 2);
            Assert.Contains(_urlService.GetAll(), u => u.Id == 1);
            Assert.Contains(_urlService.GetAll(), u => u.Id == 2);
        }
    }
}
