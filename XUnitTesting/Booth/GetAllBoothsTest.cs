using System;
using System.Collections.Generic;
using Core.Application.Implementation;
using Core.Domain;
using Moq;
using Xunit;

namespace XUnitTesting.Booth
{
    public class GetAllBoothsTest
    {
        private Mock<IRepository<Core.Entity.Booth>> mockBoothRepository = new Mock<IRepository<Core.Entity.Booth>>();

        [Fact]
        public void GetAllBooths(){

            var BoothList = new List<Core.Entity.Booth>
            {
                new Core.Entity.Booth(){
                    Id = 1,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 3,
                    Booker = null
                },
                new Core.Entity.Booth(){
                    Id = 4,
                    Booker = null
                },
                new Core.Entity.Booth(){
                    Id = 5,
                    Booker = new Core.Entity.User()
                }
            };

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => BoothList);

            var result = new BoothService(mockBoothRepository.Object).GetAll();

            Assert.Equal(BoothList, result);
        }
    }
}
