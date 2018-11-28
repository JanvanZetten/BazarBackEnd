using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace XUnitTesting.User
{
    public class UserServiceCreateTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Dictionary<int, Core.Entity.User> userDictionary = new Dictionary<int, Core.Entity.User>();
        private int nextId = 1;

        readonly IUserService _userService;
        readonly string _password = "Ab1Ab1Ab";
        readonly Core.Entity.User _user = new Core.Entity.User()
        {
            Username = "jan"
        };

        public UserServiceCreateTest()
        {
            mockUserRepository.Setup(x => x.Register(It.IsAny<Core.Entity.User>(), It.IsAny<string>())).Returns<Core.Entity.User, string>((u, p) => 
            {
                u.Id = nextId++;
                userDictionary.Add(u.Id, u);
                return userDictionary[u.Id];
            });
            mockUserRepository.Setup(x => x.UniqueUsername(It.IsAny<string>())).Returns<string>((username) =>
            {
                return !userDictionary.Values.Any(user => user.Username.ToLower() == username.ToLower());
            });

            _userService = new UserService(mockUserRepository.Object);
        }

        [Fact]
        public void CreateUserValid()
        {
            userDictionary.Clear();

            var result = _userService.Create(_user, _password);

            Assert.Equal(_user.Username, result.Username);
        }

        [Theory]
        [InlineData("ja", false)] // below minimum length
        [InlineData("jan", true)] // equals minimum length
        [InlineData("jani", true)] // above minimum length
        [InlineData("lllllllllllllllllllllllllllllllllllll39", true)] // below maximum length
        [InlineData("llllllllllllllllllllllllllllllllllllll40", true)] // equals maximum length
        [InlineData("lllllllllllllllllllllllllllllllllllllll41", false)] // above maximum length
        public void CreateUserUsernameRules(string username, bool isValid)
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Username = username
            };
            
            Core.Entity.User result = null;
            if (isValid)
            {
                result = _userService.Create(test, _password);

                Assert.Equal(test.Username, result.Username);
            }
            else
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    result = _userService.Create(test, _password);
                });

                Assert.Null(result);
            }
        }

        [Fact]
        public void CreateUserInvalidUsernameNull()
        {
            Core.Entity.User test = new Core.Entity.User();

            Core.Entity.User result = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                result = _userService.Create(test, _password);
            });

            Assert.Null(result);
        }

        [Fact]
        public void CreateUserInvalidDuplicateUsername()
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Username = "Jan"
            };

            _userService.Create(_user, _password);
            Core.Entity.User result = null;
            Assert.Throws<ArgumentException>(() =>
            {
                result = _userService.Create(test, _password);
            });

            Assert.Null(result);
        }

        [Fact]
        public void CreateUserInvalidPasswordNull()
        {
            Core.Entity.User result = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                result = _userService.Create(_user, null);
            });

            Assert.Null(result);
        }

        [Theory]
        [InlineData("Ab1Ab1A", false)] // below minimum length
        [InlineData("Ab1Ab1Ab", true)] // equals minimum length
        [InlineData("Ab1Ab1Ab1", true)] // above minimum length
        [InlineData("aaaaaaaa", false)] // only lowercase
        [InlineData("AAAAAAAA", false)] // only uppercase
        [InlineData("11111111", false)] // only numbers
        [InlineData("a1a1a1a1", false)] // only lowercase and numbers
        [InlineData("A1A1A1A1", false)] // only uppercase and numbers
        [InlineData("AbAbAbAb", false)] // only uppercase and lowercase
        [InlineData("Ablllllllllllllllllllllllllllllllllll39", true)] // below maximum length
        [InlineData("Abllllllllllllllllllllllllllllllllllll40", true)] // equals maximum length
        [InlineData("Ablllllllllllllllllllllllllllllllllllll41", false)] // above maximum length
        public void CreateUserPasswordRules(string password, bool isValid)
        {
            Core.Entity.User result = null;
            if (isValid)
            {
                result = _userService.Create(_user, password);

                Assert.Equal(_user.Username, result.Username);
            }
            else
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    result = _userService.Create(_user, password);
                });

                Assert.Null(result);
            }
        }
    }
}
