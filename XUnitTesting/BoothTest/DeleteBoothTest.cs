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
    public class DeleteBoothTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();
        private Mock<ILogService> mockLogService = new Mock<ILogService>();

        [Fact]
        public void DeleteBooth()
        {
            var booth = new Booth() { Id = 1 };
            mockBoothRepository.Setup(m => m.Delete(It.IsAny<int>())).Returns(() => booth);
            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => booth);
            var result = new BoothService(
                mockUserRepository.Object, 
                mockBoothRepository.Object, 
                mockAuthenticationService.Object, 
                mockWaitingListRepository.Object,
                mockLogService.Object)
                .Delete(booth.Id);

            Assert.Equal(booth.Id, result.Id);
        }

        [Fact]
        public void DeleteBoothWithIdZero()
        {
            var booth = new Booth() { Id = 0 };

            Assert.Throws<BoothNotFoundException>(() =>
            new BoothService(
                mockUserRepository.Object,
                mockBoothRepository.Object,
                mockAuthenticationService.Object,
                mockWaitingListRepository.Object,
                mockLogService.Object)
                .Delete(booth.Id));
        }

        [Fact]
        public void DeleteBoothWithIdNotFound()
        {
            var booth = new Booth() { Id = 50 };
            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<BoothNotFoundException>(() =>
            new BoothService(
                mockUserRepository.Object,
                mockBoothRepository.Object,
                mockAuthenticationService.Object,
                mockWaitingListRepository.Object,
                mockLogService.Object)
                .Delete(booth.Id));
        }

        [Fact]
        public void LogOnDelete()
        {
            var booth = new Booth() { Id = 1 };
            mockBoothRepository.Setup(m => m.Delete(It.IsAny<int>())).Returns(() => booth);
            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => booth);

            new BoothService(
                mockUserRepository.Object,
                mockBoothRepository.Object,
                mockAuthenticationService.Object,
                mockWaitingListRepository.Object,
                mockLogService.Object)
                .Delete(booth.Id);

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"Stand nr. 1 er blevet slettet.")),
                It.IsAny<User>()), Times.Once);
        }
    }
}
