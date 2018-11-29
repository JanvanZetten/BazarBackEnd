using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace XUnitTesting.User
{
    public class UserServiceGetTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private Dictionary<int, Core.Entity.User> userDictionary = new Dictionary<int, Core.Entity.User>();

        readonly IUserService _userService;

        readonly Core.Entity.User user1 = new Core.Entity.User()
        {
            Id = 1,
            Username = "jan"
        };
        readonly Core.Entity.User user2 = new Core.Entity.User()
        {
            Id = 2,
            Username = "alex"
        };
        readonly Core.Entity.User user3 = new Core.Entity.User()
        {
            Id = 3,
            Username = "hussein"
        };

        public UserServiceGetTest()
        {
            userDictionary.Add(user1.Id, user1);
            userDictionary.Add(user2.Id, user2);
            userDictionary.Add(user3.Id, user3);

            mockUserRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (!userDictionary.ContainsKey(id))
                    return null;
                var user = userDictionary[id];
                return user;
            });

            mockUserRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return userDictionary.Values;
            });

            _userService = new UserService(mockUserRepository.Object, mockAuthenticationService.Object);
        }

        /// <summary>
        /// Tests to make sure that the specified ID returns the correct user.
        /// </summary>
        [Fact]
        public void GetByIdValidUser()
        {
            Core.Entity.User user = _userService.GetByID(2);
            Assert.Equal(user.Id, user2.Id);

            user = _userService.GetByID(3);
            Assert.Equal(user.Id, user3.Id);
        }

        /// <summary>
        /// Tests to make sure that the application throws an exception if the specified ID doesn't exist.
        /// </summary>
        [Fact]
        public void GetByIdInvalidUser()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _userService.GetByID(4);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _userService.GetByID(0);
            });
        }

        /// <summary>
        /// Tests to make sure that the GetAll method returns the correct amount of users and the correct users.
        /// </summary>
        [Fact]
        public void GetAllValidResult()
        {
            var results = _userService.GetAll();
            Assert.Equal(3, results.Count());

            Assert.Contains(results, u => u.Username == user1.Username);
            Assert.Contains(results, u => u.Username == user2.Username);
            Assert.Contains(results, u => u.Username == user3.Username);
        }
    }
}
