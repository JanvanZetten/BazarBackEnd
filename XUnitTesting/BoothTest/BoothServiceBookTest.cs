using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class BoothServiceBookTest
    {
        readonly IBoothService _boothService;

        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();

        private Dictionary<int, User> userDictionary = new Dictionary<int, User>();
        private Dictionary<int, Booth> boothDictionary = new Dictionary<int, Booth>();

        private User user1;
        private User user2;

        private Booth booth1;
        private Booth booth2;

        private string token1 = "Hello";
        private string token2 = "Adieu";

        public BoothServiceBookTest()
        {
            user1 = new User()
            {
                Id = 1,
                Username = "jan"
            };
            user2 = new User()
            {
                Id = 2,
                Username = "hussein"
            };

            booth1 = new Booth()
            {
                Id = 1
            };
            booth2 = new Booth()
            {
                Id = 2
            };

            userDictionary.Add(1, user1);
            userDictionary.Add(2, user2);

            boothDictionary.Add(1, booth1);
            boothDictionary.Add(2, booth2);

            mockUserRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
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

            mockUserRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return userDictionary.Values;
            });

            mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (boothDictionary.ContainsKey(id))
                {
                    return boothDictionary[id];
                }
                else
                {
                    return null;
                }
            });

            mockBoothRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return boothDictionary.Values;
            });

            mockBoothRepository.Setup(x => x.Update(It.IsAny<Booth>())).Returns<Booth>((b) =>
            {
                if (b == null)
                    return null;

                if (boothDictionary.ContainsKey(b.Id))
                {
                    boothDictionary[b.Id] = b;
                    return boothDictionary[b.Id];
                }
                else
                {
                    return null;
                }
            });

            mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                if (token1 == s)
                    return user2.Username;
                else if (token2 == s)
                    return "asbamse";
                throw new ArgumentException("Invalid token");
            });

            _boothService = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object);
        }

        /// <summary>
        /// Make valid booking.
        /// </summary>
        [Fact]
        public void BookValidInput()
        {
            Booth booth = _boothService.Book(token1);

            Assert.True(boothDictionary.Values.Any(b => b.Booker.Username == user2.Username));
        }

        [Fact]
        public void BookInvalidUser()
        {
            Booth booth = _boothService.Book(token1);

            Assert.False(boothDictionary.Values.Any(b => b.Booker?.Username == user1.Username));
        }

        [Fact]
        public void BookInvalidToken()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _boothService.Book("Mojn");
            });
        }

        [Fact]
        public void BookWithUserNotFound()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _boothService.Book(token2);
            });
        }

        [Fact]
        public void BookWithNoBoothsAvailable()
        {
            Booth booth1 = _boothService.Book(token1);
            Booth booth2 = _boothService.Book(token1);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _boothService.Book(token1);
            });
        }
    }
}
