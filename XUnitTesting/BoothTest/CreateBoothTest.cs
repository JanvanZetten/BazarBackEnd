using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private Mock<ILogService> mockLogService = new Mock<ILogService>();

        [Fact]
        public void CreateValidBoothTest()
        {
            int id = 1;

            mockBoothRepository.Setup(m => m.Create(It.IsAny<List<Booth>>())).
                Returns(() => new List<Booth> {
                    new Booth { Id = id }
                });

            var result = new BoothService(null, mockBoothRepository.Object, null, null, mockLogService.Object).Create(1, new Booth());
            
            Assert.Equal(id, result.FirstOrDefault(b => b.Id == id).Id);
        }

        [Fact]
        public void CreateBoothWithIdChangeTest()
        {
            mockBoothRepository.Setup(m => m.Create(It.Is<List<Booth>>(k => k[0].Id == 0)));

            new BoothService(null, mockBoothRepository.Object, null, null, mockLogService.Object).Create(1, new Booth() { Id = 1 });

            mockBoothRepository.VerifyAll();
        }

        [Fact] 
        public void LogOnCreate()
        {
            new BoothService(null, mockBoothRepository.Object, null, null, mockLogService.Object).Create(1, new Booth());

            mockLogService.Verify(x => x.Create(It.IsAny<string>(), It.IsAny<User>()), Times.Once);
        }
    }
}
