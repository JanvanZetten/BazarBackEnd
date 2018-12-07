using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class GetAllBoothsIncludeAllTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();

        private User user1 = new User()
        {
            Id = 1,
            Username = "Test",
            PasswordHash = Encoding.ASCII.GetBytes("hash"),
            PasswordSalt = Encoding.ASCII.GetBytes("salt")
        };

        [Fact]
        public void GetAllBoothsIncludeAll()
        {
            var BoothList = new List<Booth>
            {
                new Booth(){
                    Id = 1,
                    Booker = user1
                },
                new Booth(){
                    Id = 2,
                    Booker = user1
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
                    Booker = user1
                }
            };

            mockBoothRepository.Setup(x => x.GetAllIncludeAll()).Returns(() => BoothList);

            var result = new BoothService(null, mockBoothRepository.Object, null, null).GetAllIncludeAll();

            result.ForEach(b =>
            {
                if (b.Booker != null)
                {
                    Assert.Equal(b.Booker.Id, user1.Id);
                    Assert.Equal(b.Booker.Username, user1.Username);
                    Assert.Null(b.Booker.PasswordHash);
                    Assert.Null(b.Booker.PasswordSalt);
                }
            });

            int resultCount = result.Where(b => b.Booker != null).ToList().Count;
            Assert.Equal(3, resultCount);

            Assert.Equal(BoothList.Count, result.Count);
        }
    }
}
