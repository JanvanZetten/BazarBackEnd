using System;
using System.Collections.Generic;
using Core.Application.Implementation;
using Core.Domain;
using Moq;
using Xunit;
namespace XUnitTesting.Booth
{
    public class UpdateBoothTest
    {
        private Mock<IRepository<Core.Entity.Booth>> mockBoothRepository = new Mock<IRepository<Core.Entity.Booth>>();

        public UpdateBoothTest()
        {
        }

        [Fact]
        public void UpdateBoothValidTest(){

            mockBoothRepository.Setup(m => m.Update(It.IsAny<Core.Entity.Booth>())).Returns(() => new Core.Entity.Booth());
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => new Core.Entity.Booth());

            var result = new BoothService(mockBoothRepository.Object).Update(new Core.Entity.Booth(){Id = 1});

            Assert.NotNull(result);

        }

        [Fact]
        public void NonExistingIdUpdateBoothTestExpectException(){
            mockBoothRepository.Setup(m => m.Update(It.IsAny<Core.Entity.Booth>())).Returns(() => new Core.Entity.Booth());
            mockBoothRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<ArgumentOutOfRangeException>(() => new BoothService(mockBoothRepository.Object).Update(new Core.Entity.Booth()));
        }
    }
}
