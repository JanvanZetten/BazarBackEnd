using System;
using System.Collections.Generic;
using Core.Application.Implementation;
using Core.Domain;
using Moq;
using Xunit;

namespace XUnitTesting.Booth
{
    public class GetUsersBookingTest
    {
        private Mock<IRepository<Core.Entity.Booth>> mockBoothRepository = new Mock<IRepository<Core.Entity.Booth>>();

        [Fact]
        public void GetUsersBookingSingleBookingTest(){
            var user = new Core.Entity.User() { Id = 1 };
            var booth = new Core.Entity.Booth() { Id = 1, Booker = user };

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Core.Entity.Booth>
            {
                booth,
                new Core.Entity.Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User(){Id = 2}
                }
            });

            var result = new BoothService(mockBoothRepository.Object).GetUsersBooking(user.Id);

            Assert.Equal(booth, result);

        }

        [Fact]
        public void GetUsersBookingNoBookingTest()
        {
            var user = new Core.Entity.User() { Id = 1 };

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Core.Entity.Booth>
            {
               new Core.Entity.Booth(){
                    Id = 1,
                    Booker = new Core.Entity.User(){Id = 2}
                },
                new Core.Entity.Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User(){Id = 3}
                }
            });

            var result = new BoothService(mockBoothRepository.Object).GetUsersBooking(user.Id);

            Assert.Null(result);
        }

    }
}
