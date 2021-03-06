﻿using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
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
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();
        private Mock<ILogService> mockLogService = new Mock<ILogService>();

        private Dictionary<int, User> userDictionary = new Dictionary<int, User>();
        private Dictionary<int, Booth> boothDictionary = new Dictionary<int, Booth>();

        private User user1;
        private User user2;

        private Booth booth1;
        private Booth booth2;

        private string token1 = "Hello";
        private string token2 = "Adieu";

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
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

            mockBoothRepository.Setup(x => x.GetAllIncludeAll()).Returns(() =>
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
                throw new InvalidTokenException("Invalid token");
            });

            _boothService = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object, mockLogService.Object);
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

        /// <summary>
        /// Test to throw exception when token is invalid
        /// </summary>
        [Fact]
        public void BookInvalidToken()
        {
            Assert.Throws<InvalidTokenException>(() =>
            {
                _boothService.Book("Mojn");
            });
        }

        /// <summary>
        /// Test to throw exception when user doesn't exist
        /// </summary>
        [Fact]
        public void BookWithUserNotFound()
        {
            Assert.Throws<UserNotFoundException>(() =>
            {
                _boothService.Book(token2);
            });
        }

        /// <summary>
        /// Test to throw exception when no booths remain unbooked and a booth is attempted to be booked
        /// </summary>
        [Fact]
        public void BookWithNoBoothsAvailable()
        {
            Booth booth1 = _boothService.Book(token1);
            Booth booth2 = _boothService.Book(token1);

            Assert.Throws<OnWaitingListException>(() =>
            {
                _boothService.Book(token1);
            });
        }

        /// <summary>
        /// Test to create correct log entry
        /// </summary>
        [Fact]
        public void LogOnBook()
        {
            Booth booth1 = _boothService.Book(token1);
            Booth booth2 = _boothService.Book(token1);

            Assert.Throws<OnWaitingListException>(() =>
            {
                _boothService.Book(token1);
            });

            //Called 3 times
            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"{user2.Username} har reserveret stand {booth1.Id} med tilfældig standreservering.")),
                It.Is<User>(u => u.Equals(user2))), Times.Once);

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"{user2.Username} har reserveret stand {booth2.Id} med tilfældig standreservering.")),
               It.Is<User>(u => u.Equals(user2))), Times.Once);

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"{user2.Username} har fået en plads på ventelisten.")),
               It.Is<User>(u => u.Equals(user2))), Times.Once);

        }
    }
}
