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
    public class UpdateBoothTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();

        [Fact]
        public void UpdateBoothValidTest()
        {
            mockBoothRepository.Setup(m => m.Update(It.IsAny<Booth>())).Returns(() => new Booth());
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => new Booth());

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).
                Update(new Booth(){Id = 1});

            Assert.NotNull(result);
        }

        [Fact]
        public void NonExistingIdUpdateBoothTestExpectException()
        {
            mockBoothRepository.Setup(m => m.Update(It.IsAny<Booth>())).Returns(() => new Booth());
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<BoothNotFoundException>(() => new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).
            Update(new Booth()));
        }
    }
}
