using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace XUnitTesting.UserTest
{
    public class UserServiceCreateTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Dictionary<int, User> userDictionary = new Dictionary<int, User>();
        private int nextId = 1;

        readonly IUserService _userService;
        readonly string _password = "Ab1Ab1Ab";
        readonly User _user = new User()
        {
            Username = "jan"
        };

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public UserServiceCreateTest()
        {
            mockUserRepository.Setup(x => x.Register(It.IsAny<User>(), It.IsAny<string>())).Returns<User, string>((u, p) => 
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

        /// <summary>
        /// Create user with valid information.
        /// </summary>
        [Fact]
        public void CreateUserValid()
        {
            userDictionary.Clear();

            var result = _userService.Create(_user, _password);

            Assert.Equal(_user.Username, result.Username);
        }

        /// <summary>
        /// Testing limits of username requirements.
        /// </summary>
        /// <param name="username">The test username.</param>
        /// <param name="isValid">Is the username valid.</param>
        [Theory]
        [InlineData("ja", false)] // below minimum length
        [InlineData("jan", true)] // equals minimum length
        [InlineData("jani", true)] // above minimum length
        [InlineData("lllllllllllllllllllllllllllllllllllll39", true)] // below maximum length
        [InlineData("llllllllllllllllllllllllllllllllllllll40", true)] // equals maximum length
        [InlineData("lllllllllllllllllllllllllllllllllllllll41", false)] // above maximum length
        public void CreateUserUsernameRules(string username, bool isValid)
        {
            User test = new User()
            {
                Username = username
            };
            
            User result = null;
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

        /// <summary>
        /// Test for username ArgumentNullException.
        /// </summary>
        [Fact]
        public void CreateUserInvalidUsernameNull()
        {
            User test = new User();

            User result = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                result = _userService.Create(test, _password);
            });

            Assert.Null(result);
        }

        /// <summary>
        /// Test that username duplicate is not allowed.
        /// </summary>
        [Fact]
        public void CreateUserInvalidDuplicateUsername()
        {
            User test = new User()
            {
                Username = "Jan"
            };

            _userService.Create(_user, _password);
            User result = null;
            Assert.Throws<ArgumentException>(() =>
            {
                result = _userService.Create(test, _password);
            });

            Assert.Null(result);
        }

        /// <summary>
        /// Test for username ArgumentNullException.
        /// </summary>
        [Fact]
        public void CreateUserInvalidPasswordNull()
        {
            User result = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                result = _userService.Create(_user, null);
            });

            Assert.Null(result);
        }

        /// <summary>
        /// Testing limits of password requirements.
        /// </summary>
        /// <param name="username">The test password.</param>
        /// <param name="isValid">Is the password valid.</param>
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
            User result = null;
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
