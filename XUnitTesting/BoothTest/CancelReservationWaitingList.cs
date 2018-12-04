using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class CancelReservationWaitingList
    {
        readonly IBoothService _boothService;

        private Mock<IWaitingListRepository> mockWaitingListItemRepository = new Mock<IWaitingListRepository>();
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();

        private Dictionary<int, User> userDictionary = new Dictionary<int, User>();
        private Dictionary<int, Booth> boothDictionary = new Dictionary<int, Booth>();

        private Dictionary<int, WaitingListItem> waitingListItemDictionary = new Dictionary<int, WaitingListItem>();

        private WaitingListItem wli1;
        private WaitingListItem wli2;

        private User user1;
        private User user2;

        private Booth booth1;
        private Booth booth2;

        private string token1 = "Hello";
        private string token2 = "Adieu";

        public CancelReservationWaitingList()
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

            booth1 = new Booth()
            {
                Id = 1,
                Booker = user1
            };
            booth2 = new Booth()
            {
                Id = 2,
                Booker = user2
            };
            wli1 = new WaitingListItem()
            {
                Id = 1,
                Booker = user1,
                Date = DateTime.Now
            };
            wli2 = new WaitingListItem()
            {
                Id = 2,
                Booker = user2,
                Date = DateTime.Now.AddYears(-1)
            };

            userDictionary.Add(1, user1);
            userDictionary.Add(2, user2);


            boothDictionary.Add(1, booth1);
            boothDictionary.Add(2, booth2);

            waitingListItemDictionary.Add(1, wli1);
            waitingListItemDictionary.Add(2, wli2);

            mockUserRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (userDictionary.ContainsKey(id))
                {
                    return userDictionary[id];
                }
                else
                {
                    return null;
                }
            });

            mockUserRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return userDictionary.Values;
            });

            mockBoothRepository.Setup(x => x.GetByIdIncludeAll(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (boothDictionary.ContainsKey(id))
                {
                    return boothDictionary[id];
                }
                else
                {
                    return null;
                }
            });

            mockBoothRepository.Setup(x => x.GetAllIncludeAll()).Returns(() =>
            {
                return boothDictionary.Values;
            });

            mockWaitingListItemRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return waitingListItemDictionary.Values;
            });

            mockWaitingListItemRepository.Setup(x => x.GetAllIncludeAll()).Returns(() =>
            {
                return waitingListItemDictionary.Values;
            });

            mockWaitingListItemRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns<int>((id) =>
            {
                var wli = waitingListItemDictionary[id];
                waitingListItemDictionary.Remove(id);
                return wli;
            });

            mockBoothRepository.Setup(x => x.Update(It.IsAny<Booth>())).Returns<Booth>((b) =>
            {
                if (b == null)
                    return null;

                if (boothDictionary.ContainsKey(b.Id))
                {
                    boothDictionary[b.Id] = b;
                    return boothDictionary[b.Id];
                }
                else
                {
                    return null;
                }
            });

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                if (token1 == s)
                    return user1.Username;
                else if (token2 == s)
                    return "asbamse";
                throw new InvalidTokenException("Invalid token");
            });

            _boothService = new BoothService(mockUserRepository.Object, mockBoothRepository.Object,
                mockAuthenticationService.Object, mockWaitingListItemRepository.Object);
        }
        
        /// <summary>
        /// Make sure the first entry in waiting list item repository get assigned to cancelled booth.
        /// </summary>
        [Fact]
        public void AssignBoothToWaitingListItemWithOldestDate()
        {
            var waitingListItem = waitingListItemDictionary.Values.
                FirstOrDefault( w => w.Date == waitingListItemDictionary.Values.Min(d => d.Date));
            var booth = _boothService.CancelReservation(booth1.Id, token1);

            Assert.True(booth.Booker == waitingListItem.Booker);
        }

        /// <summary>
        /// Make sure that booker is null when waiting list is empty.
        /// </summary>
        [Fact]
        public void WaitingListEmpty()
        {
            waitingListItemDictionary.Clear();
            var booth = _boothService.CancelReservation(booth1.Id, token1);
            Assert.True(booth.Booker == null);
        }
       
    }
}
