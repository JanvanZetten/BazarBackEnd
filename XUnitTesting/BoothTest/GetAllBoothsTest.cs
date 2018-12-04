using System;
using System.Collections.Generic;
using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class GetAllBoothsTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();

        [Fact]
        public void GetAllBooths()
        {
            var BoothList = new List<Booth>
            {
                new Booth(){
                    Id = 1,
                    Booker = new Core.Entity.User()
                },
                new Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User()
                },
                new Booth(){
                    Id = 3,
                    Booker = null
                },
                new Booth(){
                    Id = 4,
                    Booker = null
                },
                new Booth(){
                    Id = 5,
                    Booker = new Core.Entity.User()
                }
            };

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => BoothList);

            var result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, 
            mockAuthenticationService.Object, mockWaitingListRepository.Object).GetAll();

            Assert.Equal(BoothList, result);
        }
    }
}
