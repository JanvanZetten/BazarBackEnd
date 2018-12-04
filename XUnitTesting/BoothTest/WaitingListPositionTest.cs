using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class WaitingListPositionTest
    {
        private readonly Mock<IRepository<WaitingListItem>> mockWaitingListItemRepository = new Mock<IRepository<WaitingListItem>>();
        private readonly Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();
        private readonly Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private readonly IBoothService _service;

        IEnumerable<WaitingListItem> listWli;

       private User user1;
       private User user2;
       private User user3;
        private WaitingListItem wli1;
       private WaitingListItem wli2;

        public WaitingListPositionTest()
        {
            user1 = new User()
            {
                Id = 1,
                Username = "Alex"
            };
            user2 = new User()
            {
                Id = 2,
                Username = "Hussain"
            };
            user3 = new User()
            {
                Id = 3,
                Username = "Asbjørn"
            };
            wli1 = new WaitingListItem()
            {
                Booker = user1,
                Date = DateTime.Now,
                Id = 1
            };
            wli2 = new WaitingListItem()
            {
                Booker = user2,
                Date = DateTime.Now.AddDays(2),
                Id = 2
            };
            listWli = new List<WaitingListItem>()
            {
                wli1, 
                wli2

            };
            mockWaitingListItemRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return listWli;
            });
            mockWaitingListItemRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
            {
                return listWli.FirstOrDefault(w => w.Id == id);
            });
            

            _service = new BoothService(mockUserRepository.Object, mockBoothRepository.Object,
                mockAuthenticationService.Object, mockWaitingListItemRepository.Object);
        }

        [Fact]
        public void GetTheWaitingPosition()
        {

            var positionWli1 = _service.GetWaitingListItemPosition(user1.Id) + 1;
            var positionWli2 = _service.GetWaitingListItemPosition(user2.Id) + 1;

            Assert.True(positionWli1 < positionWli2);
        }
        [Fact]
        public void GetTheWaitingPositionInvalid()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var wli = _service.GetWaitingListItemPosition(user3.Id);
            });
                
            

        }
    }
}
