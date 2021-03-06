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
    public class BoothServiceCancelReservationTest
    {
        readonly IBoothService _boothService;

        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();
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
        public BoothServiceCancelReservationTest()
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
                Id = 1,
                Booker = user1
            };
            booth2 = new Booth()
            {
                Id = 2,
                Booker = user2
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

            mockBoothRepository.Setup(x => x.GetByIdIncludeAll(It.IsAny<int>())).Returns<int>((id) =>
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
                    return user1.Username;
                else if (token2 == s)
                    return "asbamse";
                throw new InvalidTokenException("Invalid token");
            });

            _boothService = new BoothService(
                mockUserRepository.Object,
                mockBoothRepository.Object,
                mockAuthenticationService.Object,
                mockWaitingListRepository.Object,
                mockLogService.Object);
        }

        /// <summary>
        /// Make valid Cancel.
        /// </summary>
        [Fact]
        public void CancelBookingValidInput()
        {
            Booth booth = _boothService.CancelReservation(booth1.Id, token1);

            Assert.True(booth.Id == booth1.Id);
            Assert.True(boothDictionary.Values.Any(b => b.Id == booth1.Id && b.Booker == null));
        }

        /// <summary>
        /// Test to throw exception when user attempts cancelling booking when it is not the booker on the booth
        /// </summary>
        [Fact]
        public void CancelBookingInvalidUser()
        {
            Assert.Throws<NotAllowedException>(() =>
            {
                _boothService.CancelReservation(booth2.Id, token1);
            });
        }

        /// <summary>
        /// Test to throw exception when token is invalid
        /// </summary>
        [Fact]
        public void CancelBookingInvalidToken()
        {
            Assert.Throws<InvalidTokenException>(() =>
            {
                _boothService.CancelReservation(booth1.Id, "Random");
            });
        }

        /// <summary>
        /// Test to throw exception when user doesn't exist
        /// </summary>
        [Fact]
        public void CancelBookingWithUserNotFound()
        {
            Assert.Throws<NotAllowedException>(() =>
            {
                _boothService.CancelReservation(booth1.Id, token2);
            });
        }

        /// <summary>
        /// Test to throw exception when booth doesn't exist
        /// </summary>
        [Fact]
        public void CancelBookingBoothDoesNotExist()
        {
            Assert.Throws<BoothNotFoundException>(() =>
            {
                _boothService.CancelReservation(43, token1);
            });
        }

        /// <summary>
        /// Test to create correct log entry
        /// </summary>
        [Fact]
        public void LogOnCancel()
        {
            Booth booth = _boothService.CancelReservation(booth1.Id, token1);

            booth.Booker = user1;

            mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"{booth1.Booker.Username} har annuleret deres stand nr. {booth1.Id}.")),
                It.Is<User>(u => u.Equals(booth1.Booker))), Times.Once);
        }
    }
}
