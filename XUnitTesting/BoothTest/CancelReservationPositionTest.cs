using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class CancelReservationPositionTest
    {
        User user1;
        User user2;

        WaitingListItem wli1;
        WaitingListItem wli2;

        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();
        private Mock<ILogService> mockLogService = new Mock<ILogService>();
        readonly IBoothService _boothService;

        private Dictionary<int, WaitingListItem> waitinigListDictionary = new Dictionary<int, WaitingListItem>();
        string token1 = "test1";

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public CancelReservationPositionTest()
        {
            user1 = new User()
            {
                Id = 1,
                Username = "Asbjørn"
            };
            user2 = new User()
            {
                Id = 2,
                Username = "Hussain"
            };
            wli1 = new WaitingListItem()
            {
                Id = 1,
                Booker = user1,
                Date = DateTime.Now,
            };
            wli2 = new WaitingListItem()
            {
                Id = 2,
                Booker = user2,
                Date = DateTime.Now,
            };

            waitinigListDictionary.Add(1, wli1);
            waitinigListDictionary.Add(2, wli2);

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                return user1.Username;
            });

            mockWaitingListRepository.Setup(x => x.GetAllIncludeAll()).Returns(() =>
            {
                List<WaitingListItem> list = new List<WaitingListItem>()
                {
                    wli1,
                    wli2
                };
                return list;
            });

            _boothService = new BoothService(
                mockUserRepository.Object, 
                mockBoothRepository.Object,
                mockAuthenticationService.Object,
                mockWaitingListRepository.Object,
                mockLogService.Object
                );

            mockWaitingListRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns<int>((id) =>
            {
                var value = waitinigListDictionary[id];
                waitinigListDictionary.Remove(id);
                return value;
            });
        }

        /// <summary>
        /// Test To make sure a waiting list booking was cancelled correctly
        /// </summary>
        [Fact]
        public void CancelReservationTest()
        {
            var wli = _boothService.CancelWaitingPosition(token1);

            Assert.True(wli1.Id == wli.Id);
            Assert.False(waitinigListDictionary.ContainsValue(wli1));
        }

        /// <summary>
        /// Test to create correct log entry
        /// </summary>
        [Fact]
        public void LogOnCancel()
        {
            var wli = _boothService.CancelWaitingPosition(token1);

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"{wli.Booker.Username} har afmeldt sig fra ventelisten.")),
                It.Is<User>(u => u.Equals(user1))), Times.Once);
        }
    }
}
