using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTesting.LogTest
{
    public class GetAllLogsTest
    {
        private readonly Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private readonly ILogService _service;
        private readonly Mock<ILogRepository> mockLogRepository = new Mock<ILogRepository>();

        private List<Log> list;

        public GetAllLogsTest()
        {
            list = new List<Log>
            {
                new Log {Id = 1, Message = "Asbjørn", Date = DateTime.Now},
                new Log {Id = 2, Message = "Jan", Date = DateTime.Now},
                new Log {Id = 3, Message = "Alex", Date = DateTime.Now},

            };
            mockLogRepository.Setup(x => x.GetAllIncludeAll()).Returns(() =>
            {
                return list;
            });
            _service = new LogService(mockLogRepository.Object, mockUserRepository.Object);
        }
        [Fact]
        public void GetAllLogs()
        {
            var result =_service.GetAll();
            
            Assert.Equal(list.Count, result.Count);
            Assert.Equal(list, result);
        }

        [Fact]
        public void GetLogPosition()
        {
            var result = _service.GetAll();

            Assert.True(result[0] == list[0]);
        }
    }
}
