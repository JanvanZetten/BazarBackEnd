using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
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
        private readonly Mock<IWaitingListRepository> mockWaitingListItemRepository = new Mock<IWaitingListRepository>();
        private readonly Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private readonly IBoothService _service;

        IEnumerable<WaitingListItem> listWli;

        private User user1;
        private User user2;
        private User user3;
        private WaitingListItem wli1;
        private WaitingListItem wli2;
        private string token1;
        private string token2;
        private string token3;

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public WaitingListPositionTest()
        {
            #region
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
            token1 = "token1";
            token2 = "token2";
            token3 = "token3";
            #endregion

            mockWaitingListItemRepository.Setup(x => x.GetAllIncludeAll()).Returns(() =>
            {
                return listWli;
            });
            mockWaitingListItemRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
            {
                return listWli.FirstOrDefault(w => w.Id == id);
            });
            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                if (token1 == s)
                    return user1.Username;
                if (token2 == s)
                    return user2.Username;
                if (token3 == s)
                    return user3.Username;
                return null;
            });
            

            _service = new BoothService(null, null,
                mockAuthenticationService.Object, mockWaitingListItemRepository.Object);
        }

        /// <summary>
        /// Checks for the correct waiting list item position.
        /// </summary>
        [Fact]
        public void GetTheWaitingPosition()
        {
            var positionWli1 = _service.GetWaitingListItemPosition(token1);
            var positionWli2 = _service.GetWaitingListItemPosition(token2);

            Assert.True(positionWli1 < positionWli2);
        }

        /// <summary>
        /// Checks whether it throws an exception when an user is used that doesn't have a waiting list item attached.
        /// </summary>
        [Fact]
        public void GetTheWaitingPositionInvalidInput()
        {
            Assert.Throws<NotOnWaitingListException>(() =>
            {
                var wli = _service.GetWaitingListItemPosition(token3);
            });
        }
    }
}
