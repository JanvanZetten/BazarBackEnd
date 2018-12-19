using System;
using System.Collections.Generic;
using System.Text;
using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;
using Moq;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class StateOfBookingTest
    {
        private Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private Mock<ILogService> mockLogService = new Mock<ILogService>();

        IBoothService _boothServ;

        User user = new User()
        {
            Id = 1,
            Username = "jan"
        };

        Booth booth;

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public StateOfBookingTest()
        {
            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => booth);

            mockBoothRepository.Setup(x => x.GetAllIncludeAll()).Returns(() => new List<Booth>
            {
                booth
            });

            mockUserRepository.Setup(x => x.GetAll()).Returns(() => new List<User>
            {
                user
            });

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                return user.Username;
            });

            mockWaitingListRepository.Setup(x => x.Create(It.IsAny<WaitingListItem>())).Returns(() => new WaitingListItem());

            _boothServ = new BoothService(
                mockUserRepository.Object, 
                mockBoothRepository.Object, 
                mockAuthenticationService.Object,
                mockWaitingListRepository.Object,
                mockLogService.Object);
        }

        /// <summary>
        /// Test to book booth successfully
        /// </summary>
        [Fact]
        public void ReturnBoothWhenAvailable()
        {
            booth = new Booth()
            {
                Id = 1
            };

            booth.Booker = null;

            _boothServ.Book("test");

            Assert.Equal(booth.Booker.Username, user.Username);
        }

        /// <summary>
        /// Test to throw exception when all booths are booked and booth is attempted to be booked, puts user on waiting list
        /// </summary>
        [Fact]
        public void GiveInformalExceptionWhenNoBoothAvailableAndAddToWaitingList()
        {
            User userTest = new User()
            {
                Id = 2,
                Username = "hussein"
            };

            booth = new Booth()
            {
                Id = 1,
                Booker = userTest
            };

            Assert.Throws<OnWaitingListException>(() 
                => _boothServ.Book("test"));

            mockWaitingListRepository.Verify(x => x.Create(It.IsAny<WaitingListItem>()), Times.Once());
        }

        /// <summary>
        /// Test to throw exception when user already has waiting list item
        /// </summary>
        [Fact]
        public void TheUserIsOnWaitingListExpectException()
        {
            mockWaitingListRepository.Setup(m => m.GetAll()).Returns(() => new List<WaitingListItem>() {

                new WaitingListItem()
                {
                    Booker = user
                }
            });
            
            booth = new Booth()
            {
                Id = 1,
                Booker = user
            };

            Assert.Throws<OnWaitingListException>(() => _boothServ.Book("test"));
        }
    }
}
