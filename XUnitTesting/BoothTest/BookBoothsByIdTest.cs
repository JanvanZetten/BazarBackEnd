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

        private User user;

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
                throw new InvalidTokenException("Invalid token");
            });
            
            _mockBoothRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>((id) =>
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
            
            _mockBoothRepository.Setup(x => x.Update(It.IsAny<Booth>())).Returns<Booth>((b) =>
            {
                if (b == null)
                    return null;

                if (_boothDictionary.ContainsKey(b.Id))
                {
                    _boothDictionary[b.Id] = b;
                    return _boothDictionary[b.Id];
                }
                else
                {
                    return null;
                }
            });

            _boothService = new BoothService(_mockUserRepository.Object, _mockBoothRepository.Object, _mockAuthenticationService.Object, null);
        }
        
        [Fact]
        public void AssertValidListOfBooths()
        {
            var result = _boothService.BookBoothsById(boothsValid1, token);

            boothsValid1.ForEach(b =>
            {
                Assert.Equal(user.Id, _boothDictionary[b.Id].Booker?.Id);
                Assert.Contains(result, r => r.Id == b.Id && r.Booker?.Id == user.Id);
            });

            Assert.True(result.Count == 2);


            result = _boothService.BookBoothsById(boothsValid2, token);

            Assert.Equal(user.Id, _boothDictionary[boothsValid2[0].Id].Booker?.Id);
            Assert.Contains(result, r => r.Id == boothsValid2[0].Id && r.Booker?.Id == user.Id);

            Assert.True(result.Count == 1);
        }

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
    }
}
