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
        private Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private Mock<IRepository<WaitingListItem>> mockWaitingListRepository = new Mock<IRepository<WaitingListItem>>();
        readonly IBoothService _boothService;

        private Dictionary<int, WaitingListItem> waitinigListDictionary = new Dictionary<int, WaitingListItem>();
        string token1 = "test1";
           
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

            mockWaitingListRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (waitinigListDictionary.ContainsKey(id))
                {
                    return waitinigListDictionary[id];
                }
                else
                {
                    return null;
                }
            });

            _boothService = new BoothService(mockUserRepository.Object, mockBoothRepository.Object,
                mockAuthenticationService.Object, mockWaitingListRepository.Object);
            mockWaitingListRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns<int>((id) =>
            {
                var value = waitinigListDictionary[id];
                waitinigListDictionary.Remove(id);
                return value;
            });
        }

        [Fact]
        public void CancelReservationTest()
        {
            var wli = _boothService.CancelWaitingPosition(wli1.Id, token1);

            Assert.True(wli1.Id == wli.Id);
            Assert.False(waitinigListDictionary.ContainsValue(wli1));
        }
    }
}
