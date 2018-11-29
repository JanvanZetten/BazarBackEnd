using System;
using System.Collections.Generic;
using Core.Application.Implementation;
using Core.Domain;
using Moq;
using Xunit;

namespace XUnitTesting.Booth
{
    public class CreateBoothTest
    {
        private Mock<IRepository<Core.Entity.Booth>> mockBoothRepository = new Mock<IRepository<Core.Entity.Booth>>();

        public CreateBoothTest()
        {
        }


        [Fact]
        public void CreateValidBoothTest(){

            int id = 1;

            mockBoothRepository.Setup(m => m.Create(It.IsAny<Core.Entity.Booth>())).
                Returns(() => new Core.Entity.Booth{Id = id});


            var result = new BoothService(mockBoothRepository.Object).Create(new Core.Entity.Booth());

            Assert.Equal(id, result.Id);

        }

        [Fact]
        public void CreateBoothWithIdChangeTest()
        {
            var didRun= false;
            mockBoothRepository.Setup(m => m.Create(It.Is<Core.Entity.Booth>(k => k.Id == 0))).
                Callback(() => didRun = true);

            new BoothService(mockBoothRepository.Object).Create(new Core.Entity.Booth() { Id = 1 });

            Assert.True(didRun);
           
        }
    }
}
