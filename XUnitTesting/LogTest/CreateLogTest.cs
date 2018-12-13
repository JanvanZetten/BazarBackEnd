using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

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
            _service = new LogService(mockLogRepository.Object, mockUserRepository.Object);
        }

        public void ValidLog()
        {
            Log log = new Log() { Id = 50, Date = DateTime.Today, Message = "Asbjørn elsker bjørne" };

            
        }
    }
}
