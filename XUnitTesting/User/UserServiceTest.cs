using Core.Application;
using Core.Entity;
using System;
using Xunit;

namespace XUnitTesting.User
{
    public class UserServiceTest
    {
        readonly IUserService _userService;

        [Fact]
        public void CreateUserValid()
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Username = "jan",
                PasswordHash = new byte[1],
                PasswordSalt = new byte[1]
            };

            var result = _userService.Create(test);

            Assert.Equal(test.Username, result.Username);
            Assert.Equal(test.PasswordHash, result.PasswordHash);
            Assert.Equal(test.PasswordSalt, result.PasswordSalt);
        }

        [Theory]
        [InlineData("ja", false)]
        [InlineData("jan", true)]
        [InlineData("jani", true)]
        public void CreateUserUsernameMinLength(string username, bool isValid)
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Username = username,
                PasswordHash = new byte[1],
                PasswordSalt = new byte[1]
            };
            
            Core.Entity.User result = null;
            if (isValid)
            {
                result = _userService.Create(test);

                Assert.Equal(test.Username, result.Username);
                Assert.Equal(test.PasswordHash, result.PasswordHash);
                Assert.Equal(test.PasswordSalt, result.PasswordSalt);
            }
            else
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    result = _userService.Create(test);
                });

                Assert.Null(result);
            }
        }

        [Fact]
        public void CreateUserInvalidUsernameNull()
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                PasswordHash = new byte[1],
                PasswordSalt = new byte[1]
            };

            Core.Entity.User result = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                result = _userService.Create(test);
            });

            Assert.Null(result);
        }

        [Fact]
        public void CreateUserInvalidPasswordHashNull()
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Username = "jan",
                PasswordSalt = new byte[1]
            };

            Core.Entity.User result = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                result = _userService.Create(test);
            });

            Assert.Null(result);
        }

        [Fact]
        public void CreateUserInvalidPasswordSaltNull()
        {
            Core.Entity.User test = new Core.Entity.User()
            {
                Username = "jan",
                PasswordHash = new byte[1]
            };

            Core.Entity.User result = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                result = _userService.Create(test);
            });

            Assert.Null(result);
        }
    }
}
