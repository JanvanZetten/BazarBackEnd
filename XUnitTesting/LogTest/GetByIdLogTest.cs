using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTesting.LogTest
{
    public class GetByIdLogTest
    {
        private readonly Mock<ILogRepository> mockLogRepository = new Mock<ILogRepository>();
        private readonly ILogService _service;

        private Log log = new Log() { Id = 1, Date = DateTime.Today, Message = "Asbjørn elsker bjørne", User = new User() { Id = 1 } };

        public GetByIdLogTest()
        {
            mockLogRepository.Setup(x => x.GetByIdIncludeAll(It.IsAny<int>())).Returns<int>((i) =>
            {
                if (i == 1)
                {
                    return log;
                }
                return null;
            });

            _service = new LogService(mockLogRepository.Object, null);
        }

        [Fact]
        public void ValidGetByIdTest()
        {
            var result = _service.GetById(log.Id);

            Assert.True(result.Id == log.Id);
            Assert.Equal(log.Date, result.Date);
            Assert.Equal(log.Message, result.Message);
            Assert.Equal(log.User, result.User);
        }

        [Fact]
        public void InvalidGetByIdTest()
        {
            Assert.Throws<LogNotFoundException>(() =>
            {
                _service.GetById(log.Id + 1);
            });
        }
    }
}
