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

namespace XUnitTesting.BoothTest
{
    public class BookBoothsByIdTest
    {
        private readonly Dictionary<int, Booth> _boothDictionary = new Dictionary<int, Booth>();
        private readonly Mock<IBoothRepository> _mockBoothRepository = new Mock<IBoothRepository>();
        private readonly Mock<IAuthenticationService> _mockAuthenticationService = new Mock<IAuthenticationService>();
        private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<ILogService> _mockLogService = new Mock<ILogService>();
        private readonly IBoothService _boothService;

        private Booth booth1;
        private Booth booth2;
        private Booth booth3;
        private Booth booth4;

        private List<Booth> boothsValid1;
        private List<Booth> boothsValid2;
        private List<Booth> boothsWithInvalidBooth;
        private List<Booth> boothsWithBookedBooth;

        private string token;
        private string token2;

        private User user;

        /// <summary>
        /// Setup needed mock enviroment.
        /// </summary>
        public BookBoothsByIdTest()
        {
            booth1 = new Booth()
            {
                Id = 1
            };
            booth2 = new Booth()
            {
                Id = 2
            };
            booth3 = new Booth()
            {
                Id = 3
            };
            booth4 = new Booth()
            {
                Id = 4,
                Booker = new User()
            };

            boothsValid1 = new List<Booth> {
                new Booth() { Id = 1 },
                new Booth() { Id = 2 }
            };
            boothsValid2 = new List<Booth> {
                new Booth() { Id = 3 }
            };
            boothsWithInvalidBooth = new List<Booth> {
                new Booth() { Id = 1 },
                new Booth() { Id = 5 }
            };
            boothsWithBookedBooth = new List<Booth> {
                new Booth() { Id = 1 },
                new Booth() { Id = 4, Booker = new User() }
            };

            token = "testToken";
            token2 = "test2Token";

            user = new User()
            {
                Id = 1,
                Username = "testUser"
            };

            _boothDictionary.Add(booth1.Id, booth1);
            _boothDictionary.Add(booth2.Id, booth2);
            _boothDictionary.Add(booth3.Id, booth3);
            _boothDictionary.Add(booth4.Id, booth4);

            _mockUserRepository.Setup(x => x.GetAll()).Returns(() =>
            {
                return new List<User>()
                {
                    user
                };
            });

            _mockAuthenticationService.Setup(x => x.VerifyUserFromToken(It.IsAny<string>())).Returns<string>((s) =>
            {
                if (token == s)
                    return user.Username;
                else if (token2 == s)
                    return "MojnUser";
                throw new InvalidTokenException("Invalid token");
            });
            
            _mockBoothRepository.Setup(x => x.GetByIdIncludeAll(It.IsAny<int>())).Returns<int>((id) =>
            {
                if (_boothDictionary.ContainsKey(id))
                {
                    return _boothDictionary[id];
                }
                else
                {
                    return null;
                }
            });
            
            _mockBoothRepository.Setup(x => x.Update(It.IsAny<List<Booth>>())).Returns<List<Booth>>((b) =>
            {
                List<Booth> result = new List<Booth>();
                for (int i = 0; i < b.Count; i++)
                {
                    if (_boothDictionary.ContainsKey(b[i].Id))
                    {
                        result.Add(b[i]);
                    }
                    else
                    {
                        return b;
                    }
                }
                for (int i = 0; i < b.Count; i++)
                {
                    _boothDictionary[b[i].Id] = b[i];
                }

                return result;
            });

            _boothService = new BoothService(
                _mockUserRepository.Object, 
                _mockBoothRepository.Object, 
                _mockAuthenticationService.Object, 
                null, 
                _mockLogService.Object);
        }
        
        /// <summary>
        /// Test to book booths correctly
        /// </summary>
        [Fact]
        public void AssertValidListOfBooths()
        {
            var result = _boothService.BookBoothsById(boothsValid1, token);

            boothsValid1.ForEach(b =>
            {
                Assert.Equal(user.Id, _boothDictionary[b.Id].Booker?.Id);
                Assert.True(result.Any(r => r.Id == b.Id && r.Booker?.Id == user.Id));
            });

            Assert.True(result.Count == 2);


            result = _boothService.BookBoothsById(boothsValid2, token);

            Assert.Equal(user.Id, _boothDictionary[boothsValid2[0].Id].Booker?.Id);
            Assert.True(result.Any(r => r.Id == boothsValid2[0].Id && r.Booker?.Id == user.Id));

            Assert.True(result.Count == 1);
        }

        /// <summary>
        /// Test to throw exception when list of booths contains no booths and make sure it hasn't booked any booths
        /// </summary>
        [Fact]
        public void AssertInvalidBooths()
        {
            List<Booth> result = null;
            Assert.Throws<BoothNotFoundException>(() =>
            {
                result = _boothService.BookBoothsById(boothsWithInvalidBooth, token);
            });

            Assert.Null(result);
            Assert.Null(_boothDictionary[1].Booker);
        }

        /// <summary>
        /// Test to throw exception when a booth is attempted to be booked that already has booker.
        /// Make sure no booths were booked if this happens.
        /// </summary>
        [Fact]
        public void AssertBookedBooth()
        {
            List<Booth> result = null;
            Assert.Throws<AlreadyBookedException>(() =>
            {
                result = _boothService.BookBoothsById(boothsWithBookedBooth, token);
            });

            Assert.Null(result);
            Assert.Null(_boothDictionary[1].Booker);
            Assert.True(_boothDictionary[4].Booker?.Id != user.Id);
        }

        /// <summary>
        /// Test to throw exception when given token is invalid and make sure it hasn't booked any booths
        /// </summary>
        [Fact]
        public void AssertInvalidToken()
        {
            List<Booth> result = null;
            Assert.Throws<InvalidTokenException>(() =>
            {
                result = _boothService.BookBoothsById(boothsValid1, "Mojn");
            });

            Assert.Null(result);
        }

        /// <summary>
        /// Test to throw exception when user doesn't exist and make sure it hasn't booked any booths
        /// </summary>
        [Fact]
        public void AssertInvalidUser()
        {
            List<Booth> result = null;
            Assert.Throws<UserNotFoundException>(() =>
            {
                result = _boothService.BookBoothsById(boothsValid1, token2);
            });

            Assert.Null(result);
        }

        /// <summary>
        /// Test to throw exception when the given list of booths is empty or null
        /// </summary>
        [Fact]
        public void AssertEmptyOrNull()
        {
            Assert.Throws<EmptyBookingException>(() =>
            {
                _boothService.BookBoothsById(null, token);
            });
            Assert.Throws<EmptyBookingException>(() =>
            {
                _boothService.BookBoothsById(new List<Booth>(), token);
            });
        }

        /// <summary>
        /// Test to create correct log entry
        /// </summary>
        [Fact]
        public void LogOnBook()
        {
            var result = _boothService.BookBoothsById(boothsValid1, token);

            string boothIds = "";

            foreach (var boothId in boothsValid1)
            {
                boothIds += boothId.Id + ", ";
            }

            boothIds.Substring(boothIds.Length - 2);

            _mockLogService.Verify(x => x.Create(It.Is<String>(m => m.Equals($"{user.Username} (Id: {user.Id}) har reserveret {boothsValid1.Count} stande på id {boothIds}.")),
                It.Is<User>(u => u.Equals(user))), Times.Once);
        }
    }
}
