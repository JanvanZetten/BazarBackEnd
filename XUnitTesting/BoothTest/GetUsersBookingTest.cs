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

        [Fact]
        public void GetUsersBookingSingleBookingTest()
        {
            var user = new Core.Entity.User() { Id = 1 };
            var booth = new Booth() { Id = 1, Booker = user };

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
                booth,
                new Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User(){Id = 2}
                }
            });

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object).GetUsersBooking(user.Id);

            Assert.Equal(booth, result);
        }

        [Fact]
        public void GetUsersBookingNoBookingTest()
        {
            var user = new Core.Entity.User() { Id = 1 };

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
               new Booth(){
                    Id = 1,
                    Booker = new Core.Entity.User(){Id = 2}
                },
                new Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User(){Id = 3}
                }
            });

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object).GetUsersBooking(user.Id);

            Assert.Null(result);
        }
    }
}