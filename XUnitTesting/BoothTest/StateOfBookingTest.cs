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
        private Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();

        IBoothService _boothServ;

        User user = new User()
        {
            Id = 1,
            Username = "jan"
        };

        Booth booth;

        public StateOfBookingTest()
        {


            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
                booth
            });

            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => booth);

            mockUserRepository.Setup(x => x.GetAll()).Returns(() => new List<User>
            {
                user
            });

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                return user.Username;
            });

            mockWaitingListRepository.Setup(x => x.Create(It.IsAny<WaitingListItem>())).Returns(() => new WaitingListItem());

            _boothServ = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object);
        }

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

        [Fact]
        public void TheUserIsAlreadyOnWaitingListExpectException()
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

            Assert.Throws<NotSupportedException>(() => _boothServ.Book("test"));

        }
    }
}
