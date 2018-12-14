using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace XUnitTesting.ResetAllTest
{
    public class ResetAllTest
    {
        Mock<IResetRepository> mockResetRepository = new Mock<IResetRepository>();
        Mock<IAuthenticationService> mockAuthentication = new Mock<IAuthenticationService>();
        Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();

        private readonly IResetService _service;
        Booth booth1;
        Booth booth2;
        Booth booth3;
        List<Booth> booths;
        List<User> users;
        User user1;
        User user2;

        string token1 = "Alex";
        string token2 = "Jonas";

        public ResetAllTest()
        {
            
            user1 = new User() { Id = 1, IsAdmin = true, Username = "Alex" };
            user2 = new User() { Id = 2, IsAdmin = false, Username = "Jonas" };
            users = new List<User>()
            {
                user1, user2
            };

            booths = new List<Booth>()
            {
                new Booth{ Id = 1, Booker = new User{ Id = 1, Username = "Alex"} },
                new Booth{ Id = 2, Booker = new User{ Id = 2, Username = "Hussain"} },
                new Booth{ Id = 3, Booker = null }
            };
            booth1 = booths[0];
            booth2 = booths[1];
            booth3 = booths[2];

            mockResetRepository.Setup(x => x.Reset()).Returns(() =>
            {
                booths.ForEach(b => b.Booker = null);
                return $"All {booths.Count} stande er blevet slettet";
            });

            mockAuthentication.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((token) =>
            {
                if(token == "Alex")
                {
                    return user1.Username;
                }
                if(token == "Jonas")
                {
                    return user2.Username;
                }
                if(token == "Saftevand")
                {
                    return "Saftevand";
                }
                throw new InvalidTokenException();
            });

            mockUserRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return users;
            });
            _service = new ResetService(mockResetRepository.Object, mockAuthentication.Object, mockUserRepository.Object);
            
        }

        [Fact]
        public void ResetEmptyBoothList()
        {
            booths.Clear();
            _service.ResetAll(token1);
            Assert.True(booths.Count() == 0);
        }

        [Fact]
        public void ResetBooths()
        {
            _service.ResetAll(this.token1);

            Assert.Equal(booths.Where(b => b.Booker == null).Count(), booths.Count());
        }

        [Fact]
        public void ResetBoothInvalidToken()
        {
            Assert.Throws<InvalidTokenException>(() =>
            {
                _service.ResetAll("ALEX1234");
            });
        }

        [Fact]
        public void ResetBoothUserNotFound()
        {
            Assert.Throws<UserNotFoundException>(() =>
            {
                _service.ResetAll("Saftevand");
            });
        }

        [Fact]
        public void ResetBoothUserIsNotAdmin()
        {
            Assert.Throws<NotAllowedException>(() =>
            {
                _service.ResetAll(user2.Username);
            });
        }

    }
}
