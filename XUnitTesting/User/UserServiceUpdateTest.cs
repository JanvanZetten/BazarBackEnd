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
    public class UserServiceUpdateTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private Dictionary<int, Core.Entity.User> userDictionary = new Dictionary<int, Core.Entity.User>();

        readonly IUserService _userService;
        readonly Core.Entity.User _user = new Core.Entity.User()
        {
            Id = 1,
            Username = "jan"
        };
        readonly Core.Entity.User _user2 = new Core.Entity.User()
        {
            Id = 2,
            Username = "hussein"
        };

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public UserServiceUpdateTest()
        {
            userDictionary.Add(1, _user);
            userDictionary.Add(2, _user2);

            mockUserRepository.Setup(x => x.GetById(It.IsAny <int>())).Returns<int>((id) =>
            {
                if (userDictionary.ContainsKey(id))
                {
                    return userDictionary[id];
                }
                else
                {
                    return null;
                }
            });

            mockUserRepository.Setup(x => x.Update(It.IsAny<Core.Entity.User>())).Returns<Core.Entity.User>((u) => 
            {
                if (u == null)
                    return null;

                if(userDictionary.ContainsKey(u.Id))
                {
                    userDictionary[u.Id] = u;
                    return userDictionary[u.Id];
                }
                else
                {
                    return null;
                }
            });
            
            mockUserRepository.Setup(x => x.UniqueUsername(It.IsAny<string>())).Returns<string>((username) =>
            {
                return !userDictionary.Values.Any(user => user.Username.ToLower() == username.ToLower());
            });

            _userService = new UserService(mockUserRepository.Object, mockAuthenticationService.Object);
        }

        /// <summary>
        /// Update user with valid information. Test if you can use your old username.
        /// </summary>
        [Fact]
        public void UpdateUserValid()
        {
            var test = new Core.Entity.User()
            {
                Id = 1,
                Username = "alex"
            };

            var result = _userService.Update(test);
            Assert.Equal(test.Username, result.Username);

            result = _userService.Update(test);
            Assert.Equal(test.Username, result.Username);
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
        public void UpdateUserUsernameRules(string username, bool isValid)
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Id = 1,
                Username = username
            };
            
            Core.Entity.User result = null;
            if (isValid)
            {
                result = _userService.Update(test);

                Assert.Equal(test.Username, result.Username);
            }
            else
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    result = _userService.Update(test);
                });

                Assert.Null(result);
            }
        }

        /// <summary>
        /// Test for username ArgumentNullException.
        /// </summary>
        [Fact]
        public void UpdateUserInvalidUsernameNull()
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Id = 1
            };

            Core.Entity.User result = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                result = _userService.Update(test);
            });

            Assert.Null(result);
        }

        /// <summary>
        /// Test that username duplicate is not allowed.
        /// </summary>
        [Fact]
        public void UpdateUserInvalidDuplicateUsername()
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Id = 1,
                Username = "hussein"
            };
            
            Core.Entity.User result = null;
            Assert.Throws<ArgumentException>(() =>
            {
                result = _userService.Update(test);
            });

            Assert.Null(result);
        }
        
        /// <summary>
        /// Test id not available
        /// </summary>
        [Fact]
        public void UpdateInvalidIdInput()
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Id = 3,
                Username = "alex"
            };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _userService.Update(test);
            });
        }
    }
}
