using System;
using System.Collections.Generic;
using Core.Application.Implementation;
using Core.Domain;
using Moq;
using Xunit;

namespace XUnitTesting.Booth
{
    public class DeleteBoothTest
    {
        private Mock<IRepository<Core.Entity.Booth>> mockBoothRepository = new Mock<IRepository<Core.Entity.Booth>>();

        [Fact]
        public void DeleteBooth()
        {
            var booth = new Core.Entity.Booth() { Id = 1 };
            mockBoothRepository.Setup(m => m.Delete(It.IsAny<int>())).Returns(() => booth);
            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => booth);
            var result = new BoothService(mockBoothRepository.Object).Delete(booth.Id);

            Assert.Equal(booth.Id, result.Id);
        }

        [Fact]
        public void DeleteBoothWithIdZero()
        {
            var booth = new Core.Entity.Booth() { Id = 0 };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BoothService(mockBoothRepository.Object).Delete(booth.Id));
        }

        [Fact]
        public void DeleteBoothWithIdNotFound()
        {
            var booth = new Core.Entity.Booth() { Id = 50 };
            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(() => null);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BoothService(mockBoothRepository.Object).Delete(booth.Id));
        }



    }
}
