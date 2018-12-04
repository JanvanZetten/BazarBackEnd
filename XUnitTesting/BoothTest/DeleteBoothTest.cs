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
        private Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();

        [Fact]
        public void DeleteBooth()
        {
            var booth = new Booth() { Id = 1 };
            mockBoothRepository.Setup(m => m.Delete(It.IsAny<int>())).Returns(() => booth);
            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => booth);
            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).Delete(booth.Id);

            Assert.Equal(booth.Id, result.Id);
        }

        [Fact]
        public void DeleteBoothWithIdZero()
        {
            var booth = new Booth() { Id = 0 };

            Assert.Throws<BoothNotFoundException>(() =>
            new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).
            Delete(booth.Id));
        }

        [Fact]
        public void DeleteBoothWithIdNotFound()
        {
            var booth = new Booth() { Id = 50 };
            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<BoothNotFoundException>(() =>
            new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).
            Delete(booth.Id));
        }
    }
}
