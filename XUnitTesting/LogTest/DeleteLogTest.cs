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
    public class DeleteLogTest
    {
        private readonly Mock<ILogRepository> mockLogRepository = new Mock<ILogRepository>();
        private readonly ILogService _service;
        
        private Log log = new Log() { Id = 1, Date = DateTime.Today, Message = "Asbjørn elsker bjørne" };

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public DeleteLogTest()
        {
            mockLogRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns<int>((i) =>
            {
                if(i == 1)
                {
                    return log;
                }
                return null;
            });

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

        /// <summary>
        /// Test to delete log successfully
        /// </summary>
        [Fact]
        public void ValidDeleteLog()
        {
            var result = _service.Delete(log.Id);
            
            Assert.True(result.Id == log.Id);
            Assert.Equal(log.Date, result.Date);
            Assert.Equal(log.Message, result.Message);
            Assert.Equal(log.User, result.User);
        }

        /// <summary>
        /// Test to throw exception when log doesn't exist
        /// </summary>
        [Fact]
        public void InvalidDeleteLog()
        {
            Assert.Throws<LogNotFoundException>(() => 
            {
                _service.Delete(log.Id + 1);
            });
        }
    }
}
