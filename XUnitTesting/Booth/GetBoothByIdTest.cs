using System;
using System.Collections.Generic;
using Core.Application.Implementation;
using Core.Domain;
using Moq;
using Xunit;
namespace XUnitTesting.Booth
{
    public class GetBoothByIdTest
    {
        private Mock<IRepository<Core.Entity.Booth>> mockBoothRepository = new Mock<IRepository<Core.Entity.Booth>>();

        public GetBoothByIdTest()
        {
        }

        [Fact]
        public void GetBoothByIdValidTest()
        {
            var booth = new Core.Entity.Booth(){Id = 1};
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => booth);

            var result = new BoothService(mockBoothRepository.Object).GetById(1);

            Assert.Equal(booth, result);
        }

        [Fact]
        public void GetBoothByIdTestIdToLowExpectException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockBoothRepository.Object).GetById(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockBoothRepository.Object).GetById(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockBoothRepository.Object).GetById(-10));
        }

        [Fact]
        public void GetBoothByIdTestIdDoesNotExistExpectException()
        {
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockBoothRepository.Object).GetById(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockBoothRepository.Object).GetById(10));
        }
    }
}
