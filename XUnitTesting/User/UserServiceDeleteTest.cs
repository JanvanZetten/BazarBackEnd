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
    public class UserServiceDeleteTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private Dictionary<int, Core.Entity.User> userDictionary = new Dictionary<int, Core.Entity.User>();

        readonly IUserService _userService;

        Core.Entity.User user1 = new Core.Entity.User
        {
            Username = "Bobby",
            Id = 1
        };
        Core.Entity.User user2 = new Core.Entity.User
        {
            Username = "Grego",
            Id = 2
        };

        public UserServiceDeleteTest()
        {
            mockUserRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (!userDictionary.ContainsKey(id))
                    return null;
                var user = userDictionary[id];
                userDictionary.Remove(id);
                return user;
            });

            _userService = new UserService(mockUserRepository.Object, mockAuthenticationService.Object);
        }

        /// <summary>
        /// Tests to make sure that the specified user is deleted if it exists.
        /// </summary>
        [Fact]
        public void DeleteValidInput()
        {
            userDictionary.Clear();
            userDictionary.Add(user1.Id, user1);
            userDictionary.Add(user2.Id, user2);

            Core.Entity.User deletedUser = _userService.Delete(user1.Id);

            Assert.True(!userDictionary.Values.Any(u => u.Id == user1.Id));
            Assert.Equal(user1.Username, deletedUser.Username);
            Assert.Equal(user1.Id, deletedUser.Id);
        }

        /// <summary>
        /// Tests to make sure that no users were deleted if an invalid ID is given, and that an exception is thrown.
        /// </summary>
        [Fact]
        public void DeleteInvalidInput()
        {
            userDictionary.Clear();
            userDictionary.Add(user1.Id, user1);
            userDictionary.Add(user2.Id, user2);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _userService.Delete(3);
            });
            Assert.Equal(2, userDictionary.Count);
        }
    }
}
    