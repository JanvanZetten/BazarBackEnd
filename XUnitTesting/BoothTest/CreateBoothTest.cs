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
    public class CreateBoothTest
    {
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();

        [Fact]
        public void CreateValidBoothTest()
        {
            int id = 1;

            mockBoothRepository.Setup(m => m.Create(It.IsAny<Booth>())).
                Returns(() => new Booth{Id = id});

            var result = new BoothService(null, mockBoothRepository.Object, null, null).Create(new Booth());

            Assert.Equal(id, result.Id);
        }

        [Fact]
        public void CreateBoothWithIdChangeTest()
        {
            var didRun= false;
            mockBoothRepository.Setup(m => m.Create(It.Is<Booth>(k => k.Id == 0))).
                Callback(() => didRun = true);

            new BoothService(null, mockBoothRepository.Object, null, null).Create(new Booth() { Id = 1 });

            Assert.True(didRun);
        }
    }
}
