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
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<ILogService> mockLogService = new Mock<ILogService>();

        [Fact]
        public void UpdateBoothValidTest()
        {
            var user = new User()
            {
                Id = 1,
                Username = "Bent"
            };

            mockBoothRepository.Setup(m => m.Update(It.IsAny<Booth>())).Returns(() => new Booth());
            mockBoothRepository.Setup(m => m.GetByIdIncludeAll(It.IsAny<int>())).Returns(() => new Booth());
            mockUserRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => user);

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, null, null, mockLogService.Object)
                .Update(new Booth()
                {
                    Id = 1,
                    Booker = user
                });

            Assert.NotNull(result);
        }

        [Fact]
        public void NonExistingIdUpdateBoothTestExpectException()
        {
            mockBoothRepository.Setup(m => m.Update(It.IsAny<Booth>())).Returns(() => new Booth());
            mockBoothRepository.Setup(m => m.GetByIdIncludeAll(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<BoothNotFoundException>(() => 
                new BoothService(null, mockBoothRepository.Object, null, null, mockLogService.Object)
                .Update(new Booth())
            );
        }

        [Fact]
        public void LogOnUpdateWithBoothBookerSetAsNull()
        {
            var user = new User()
            {
                Id = 1,
                Username = "Bent"
            };

            mockBoothRepository.Setup(m => m.Update(It.IsAny<Booth>())).Returns(() => new Booth());
            mockBoothRepository.Setup(m => m.GetByIdIncludeAll(It.IsAny<int>())).Returns(() => new Booth());
            mockUserRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => user);

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, null, null, mockLogService.Object)
                .Update(new Booth()
                {
                    Id = 1,
                    Booker = user
                });

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"Stand nr. 1 er blevet opdateret til at have standholder Bent.")),
                It.Is<User>(u => u.Equals(user))), Times.Once);
        }

        [Fact]
        public void LogOnUpdateWithBoothBookerNotNull()
        {
            var initUser = new User()
            {
                Id = 2,
                Username = "Jørgen"
            };
            var user = new User()
            {
                Id = 1,
                Username = "Bent"
            };

            mockBoothRepository.Setup(m => m.Update(It.IsAny<Booth>())).Returns(() => new Booth());
            mockBoothRepository.Setup(m => m.GetByIdIncludeAll(It.IsAny<int>())).Returns(() => new Booth()
            {
                Id = 1,
                Booker = initUser
            });
            mockUserRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => user);

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, null, null, mockLogService.Object)
                .Update(new Booth()
                {
                    Id = 1,
                    Booker = user
                });

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"Stand nr. 1 er blevet opdateret til at have standholder {user.Username}. Gamle standholder: {initUser.Username} (Id: {initUser.Id})")),
                It.Is<User>(u => u.Equals(user))), Times.Once);
        }

        [Fact]
        public void LogOnUpdateWithBookerIdBeingSetAsZero()
        {
            var initUser = new User()
            {
                Id = 2,
                Username = "Jørgen"
            };

            mockBoothRepository.Setup(m => m.Update(It.IsAny<Booth>())).Returns(() => new Booth());
            mockBoothRepository.Setup(m => m.GetByIdIncludeAll(It.IsAny<int>())).Returns(() => new Booth()
            {
                Id = 1,
                Booker = initUser
            });

            var user = new User()
            {
                Id = 0
            };

            var result = new BoothService(null, mockBoothRepository.Object, null, null, mockLogService.Object)
                .Update(new Booth()
                {
                    Id = 1,
                    Booker = user
                });

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"Stand nr. 1 er blevet opdateret til ikke at have en standholder.")),
                It.IsAny<User>()), Times.Once);
        } 
    }
}
