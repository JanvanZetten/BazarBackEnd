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

        [Fact]
        public void ValidLog()
        {
            Log log = new Log() { Id = 50, Date = DateTime.Today, Message = "Asbjørn elsker bjørne" };

            var result = _service.Create(log);

            Assert.True(result.Id != 50 && result.Id == 1);
            Assert.Equal(log.Message, result.Message);
            Assert.Equal(log.Date, result.Date);
            Assert.Null(result.User);
        }

        [Fact]
        public void LogWithDateSet()
        {
            Log log = new Log() { Id = 50, Message = "Asbjørn elsker bjørne" };

            var result = _service.Create(log);
            Assert.True(result.Date < DateTime.Now.AddDays(1)
                && result.Date > DateTime.Now.AddDays(-1));
        }

        [Fact]
        public void InvalidMessageLog()
        {
            Log log = new Log() { Id = 50, Date = DateTime.Today };

            Assert.Throws<InputNotValidException>(() =>
            {
                _service.Create(log);
            });
        }

        [Fact]
        public void InvalidUserLog()
        {
            Log log = new Log() { Id = 50, Date = DateTime.Today, Message = "Asbjørn elsker bjørne", User = new User() { Id=50 } };

            Assert.Throws<UserNotFoundException>(() =>
            {
                _service.Create(log);
            });
        }

        [Fact]
        public void ValidUserLog()
        {
            Log log = new Log() { Id = 50, Date = DateTime.Today, Message = "Asbjørn elsker bjørne", User = new User() { Id = 1 } };
            var result = _service.Create(log);

            Assert.True(result.Id != 50 && result.Id == 1);
            Assert.Equal(log.Message, result.Message);
            Assert.Equal(log.Date, result.Date);
            Assert.Equal(log.User.Id, result.User.Id);
        }

        [Fact]
        public void NullLog()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                _service.Create(null);
            });
        }

        [Fact]
        public void LogWithIdSet()
        {
            Log log = new Log() { Id = 50, Date = DateTime.Today, Message = "Asbjørn elsker bjørne" };

            mockLogRepository.Setup(x => x.Create(It.IsAny<Log>())).Returns<Log>((l) =>
            {
                dictionary.Add(l.Id, l);
                return dictionary[l.Id];
            });

            var result = _service.Create(log);

            Assert.True(result.Id == 0);
        }
    }
}
