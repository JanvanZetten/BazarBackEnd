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
    public class GetUsersBookingTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();

        /// <summary>
        /// Test to return corred booths that are booked for the user
        /// </summary>
        [Fact]
        public void GetUsersBookingSingleBookingTest()
        {
            var user = new User() { Id = 1 };
            var booth1 = new Booth() { Id = 1, Booker = user };
            var booth2 = new Booth() { Id = 2, Booker = user };

            mockUserRepository.Setup(x => x.GetAll()).Returns(() => new List<User>
            {
                user
            });

            mockBoothRepository.Setup(x => x.GetAllIncludeAll()).Returns(() => new List<Booth>
            {
                booth1,
                booth2,
                new Booth(){
                    Id = 2,
                    Booker = new User(){Id = 2}
                }
            });

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                return user.Username;
            });

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object)
                .GetUsersBooking("");

            Assert.Contains(booth1, result);
            Assert.Contains(booth2, result);
        }

        /// <summary>
        /// Test to throw exception when a booth doesn't exist
        /// </summary>
        [Fact]
        public void GetUsersBookingNoBookingTest()
        {
            var user = new User() { Id = 1 };

            mockUserRepository.Setup(x => x.GetAll()).Returns(() => new List<User>
            {
                user
            });

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
               new Booth(){
                    Id = 1,
                    Booker = new User(){Id = 2}
                },
                new Booth(){
                    Id = 2,
                    Booker = new User(){Id = 3}
                }
            });

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                return user.Username;
            });
            
            Assert.Throws<NoBookingsFoundException>(() =>
                new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object)
                .GetUsersBooking("")
            );
        }
    }
}
