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

namespace XUnitTesting.BoothTest
{
    public class AddToWaitingListTest
    {
        private readonly Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<IWaitingListRepository> mockWaitingListItemRepository = new Mock<IWaitingListRepository>();
        private readonly Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private readonly Mock<ILogService> mockLogService = new Mock<ILogService>();
        private readonly IBoothService _service;

        private List<WaitingListItem> waitingList = new List<WaitingListItem>();

        private User user1;
        private User user2;

        private string token1 = "Hello";
        private string token2 = "Adieu";
        private string token3 = "Bobby";

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public AddToWaitingListTest()
        {
            user1 = new User()
            {
                Id = 1,
                Username = "jan"
            };
            user2 = new User()
            {
                Id = 2,
                Username = "hussein"
            };

            mockWaitingListItemRepository.Setup(x => x.Create(It.IsAny<WaitingListItem>())).Returns<WaitingListItem>((w) =>
            {
                waitingList.Add(w);
                return w;
            });

            mockUserRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return new List<User>()
                {
                    user1,
                    user2
                };
            });

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                if (token1 == s)
                    return user1.Username;
                if (token2 == s)
                    return user2.Username;
                if (token3 == s)
                    return "asbamse";
                throw new InvalidTokenException();
            });

            _service = new BoothService(
                mockUserRepository.Object, 
                null, 
                mockAuthenticationService.Object, 
                mockWaitingListItemRepository.Object, 
                mockLogService.Object);
        }

        /// <summary>
        /// Test to make sure waiting list items are created
        /// </summary>
        [Fact]
        public void AssertWaitingListItemCreated()
        {
            var user1added = _service.AddToWaitingList("Hello");
            var user2added = _service.AddToWaitingList("Adieu");

            Assert.True(waitingList.Count == 2);
        }

        /// <summary>
        /// Test to throw exception when using bad token
        /// </summary>
        [Fact]
        public void AssertThrowsWhenInvalidToken()
        {
            Assert.Throws<InvalidTokenException>(() =>
            {
                _service.AddToWaitingList("Mojn");
            });
        }

        /// <summary>
        /// Test to throw exception when user doesn't exist
        /// </summary>
        [Fact]
        public void AssertThrowsWhenUserNotFound()
        {
            Assert.Throws<UserNotFoundException>(() =>
            {
                _service.AddToWaitingList(token3);
            });
        }

        /// <summary>
        /// Test to create correct log entry
        /// </summary>
        [Fact]
        public void LogOnWaitingListAdd()
        {
            var user1added = _service.AddToWaitingList("Hello");

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"{user1.Username} er blevet tilføjet på ventleisten. Ventelist id er {user1added.Id}")),
                It.Is<User>(u => u.Equals(user1))), Times.Once);
        }
    }
}
