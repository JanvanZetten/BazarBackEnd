using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace XUnitTesting.UserTest
{
    public class UserServiceGetTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Dictionary<int, User> userDictionary = new Dictionary<int, User>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();

        readonly IUserService _userService;

        readonly User user1 = new User()
        {
            Id = 1,
            Username = "jan"
        };
        readonly User user2 = new User()
        {
            Id = 2,
            Username = "alex"
        };
        readonly User user3 = new User()
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
            User user = _userService.GetByID(2);
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
