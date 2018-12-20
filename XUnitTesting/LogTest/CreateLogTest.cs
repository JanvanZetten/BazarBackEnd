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
    public class CreateLogTest
    {
        private readonly Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private readonly ILogService _service;
        private readonly Mock<ILogRepository> mockLogRepository = new Mock<ILogRepository>();
        private int id = 1;
        private Dictionary<int, Log> dictionary = new Dictionary<int, Log>();

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public CreateLogTest()
        {
            mockLogRepository.Setup(x => x.Create(It.IsAny<Log>())).Returns<Log>((l) =>
            {
                l.Id = id++;
                dictionary.Add(l.Id, l);
                return dictionary[l.Id];
                
            });


            mockUserRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((i) =>
            {
                if(i == 1)
                {
                    return new User() { Id = 1, Username = "Hussein" };
                }
                throw new UserNotFoundException();
            });

            _service = new LogService(mockLogRepository.Object, mockUserRepository.Object);
        }

        /// <summary>
        /// Test to create a valid log entry without user
        /// </summary>
        [Fact]
        public void ValidLog()
        {
            Log log = new Log() { Id = 50, Message = "Asbjørn elsker bjørne" };

            var result = _service.Create(log.Message);

            Assert.True(result.Id != 50 && result.Id == 1);
            Assert.Equal(log.Message, result.Message);
            Assert.Null(result.User);
        }

        /// <summary>
        /// Test to make sure date is correct
        /// </summary>
        [Fact]
        public void LogWithDateSet()
        {
            Log log = new Log() { Id = 50, Message = "Asbjørn elsker bjørne" };

            var result = _service.Create(log.Message);
            Assert.True(result.Date < DateTime.Now.AddDays(1)
                && result.Date > DateTime.Now.AddDays(-1));
        }

        /// <summary>
        /// Test to throw exception when log entry is empty
        /// </summary>
        [Fact]
        public void InvalidMessageLog()
        {
            Assert.Throws<InputNotValidException>(() =>
            {
                _service.Create(null);
            });
        }

        /// <summary>
        /// Test to throw exception when user doesn't exist
        /// </summary>
        [Fact]
        public void InvalidUserLog()
        {
            Log log = new Log() { Id = 50, Message = "Asbjørn elsker bjørne", User = new User() { Id=50 } };

            Assert.Throws<UserNotFoundException>(() =>
            {
                _service.Create(log.Message, log.User);
            });
        }

        /// <summary>
        /// Test to create valid log entry with user
        /// </summary>
        [Fact]
        public void ValidUserLog()
        {
            Log log = new Log() { Id = 50, Message = "Asbjørn elsker bjørne", User = new User() { Id = 1 } };
            var result = _service.Create(log.Message, log.User);

            Assert.True(result.Id != 50 && result.Id == 1);
            Assert.Equal(log.Message, result.Message);
            Assert.Equal(log.User.Id, result.User.Id);
        }

        /// <summary>
        /// Test to make sure ID is set automatically
        /// </summary>
        [Fact]
        public void LogWithIdSet()
        {;

           var message = "Asbjørn elsker bjørne";
            mockLogRepository.Setup(x => x.Create(It.IsAny<Log>())).Returns<Log>((l) =>
            {
                dictionary.Add(l.Id, l);
                return dictionary[l.Id];
            });

            var result = _service.Create(message);

            Assert.True(result.Id == 0);
        }
    }
}
