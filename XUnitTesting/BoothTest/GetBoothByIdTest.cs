using System;
using System.Collections.Generic;
using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using Xunit;
namespace XUnitTesting.BoothTest
{
    public class GetBoothByIdTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();

        [Fact]
        public void GetBoothByIdValidTest()
        {
            var booth = new Booth(){Id = 1};
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => booth);

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).GetById(1);

            Assert.Equal(booth, result);
        }

        [Fact]
        public void GetBoothByIdTestIdToLowExpectException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).GetById(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).GetById(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).GetById(-10));
        }

        [Fact]
        public void GetBoothByIdTestIdDoesNotExistExpectException()
        {
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).GetById(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).GetById(10));
        }
    }
}
