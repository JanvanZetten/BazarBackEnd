﻿using System;
using System.Collections.Generic;
using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class GetUsersBookingTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IRepository<WaitingListItem>> mockWaitingListRepository = new Mock<IRepository<WaitingListItem>>();

        [Fact]
        public void GetUsersBookingSingleBookingTest()
        {
            var user = new User() { Id = 1 };
            var booth = new Booth() { Id = 1, Booker = user };

            mockUserRepository.Setup(x => x.GetAll()).Returns(() => new List<User>
            {
                user
            });

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
                booth,
                new Booth(){
                    Id = 2,
                    Booker = new User(){Id = 2}
                }
            });

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                return user.Username;
            });

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).GetUsersBooking("");

            Assert.Equal(booth, result);
        }

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

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).GetUsersBooking("");

            Assert.Null(result);
        }
    }
}
