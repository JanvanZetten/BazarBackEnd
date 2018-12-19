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
    public class GetURLByIdTest
    {
        private Mock<IImageURLRepository> mockURLRepository = new Mock<IImageURLRepository>();

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
        public GetURLByIdTest()
        {
            mockURLRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (id == 1)
                    return url1;
                else if (id == 2)
                    return url2;
                else
                    return null;
            });
            _urlService = new ImageURLService(mockURLRepository.Object);
        }

        /// <summary>
        /// Test to return all the correct URLs
        /// </summary>
        [Fact]
        public void AssertGetReturnsCorrectURL()
        {
            Assert.True(_urlService.GetById(1) == url1);
            Assert.True(_urlService.GetById(2) == url2);
            Assert.True(_urlService.GetById(2) != url1);
        }

        [InlineData(0)]
        [InlineData(3)]
        /// <summary>
        /// Test to throw exceptions when URLs don't exist
        /// </summary>
        [Theory]
        public void AssertThrowsExceptionWhenInvalidURLId(int id)
        {
            Assert.Throws<ImageURLNotFoundException>(() =>
            {
                _urlService.GetById(id);
            });
        }
    }
}
