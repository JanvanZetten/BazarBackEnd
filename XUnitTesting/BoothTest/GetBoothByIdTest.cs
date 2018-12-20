using System;
using System.Collections.Generic;
using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
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

        /// <summary>
        /// Test to make sure returns correct booth
        /// </summary>
        [Fact]
        public void GetBoothByIdValidTest()
        {
            var booth = new Booth(){Id = 1};
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => booth);

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object)
                .GetById(1);

            Assert.Equal(booth, result);
        }

        /// <summary>
        /// Test to throw exception when ID too low
        /// </summary>
        [Fact]
        public void GetBoothByIdTestIdToLowExpectException()
        {
            Assert.Throws<BoothNotFoundException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object)
            .GetById(0));

            Assert.Throws<BoothNotFoundException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object)
            .GetById(-1));

            Assert.Throws<BoothNotFoundException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object)
            .GetById(-10));
        }

        /// <summary>
        /// Test to throw exception when booth doesn't exist
        /// </summary>
        [Fact]
        public void GetBoothByIdTestIdDoesNotExistExpectException()
        {
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<BoothNotFoundException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object)
            .GetById(1));

            Assert.Throws<BoothNotFoundException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object)
            .GetById(10));
        }
    }
}
