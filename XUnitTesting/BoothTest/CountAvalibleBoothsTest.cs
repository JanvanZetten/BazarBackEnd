﻿using System;
using System.Collections.Generic;
using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class CountAvalibleBoothsTest
    {
        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();
        private Mock<IAuthenticationService> mockAuthenticationService = new Mock<IAuthenticationService>();
        private static Mock<IWaitingListRepository> mockWaitingListRepository = new Mock<IWaitingListRepository>();

        [Fact]
        public void TestCountAmountOfWaitingListItems()
        {
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
                new Booth(){
                    Id = 1,
                    Booker = null
                },
                new Booth(){
                    Id = 2,
                    Booker = new User()
                },
                new Booth(){
                    Id = 3,
                    Booker = null
                },
                new Booth(){
                    Id = 4,
                    Booker = null
                },
                new Booth(){
                    Id = 5,
                    Booker = new User()
                }
            });

            int result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).CountAvailableBooths();

            Assert.Equal(3, result);
        }

        [Fact]
        public void testCountNone()
        {
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
                new Booth(){
                    Id = 1,
                    Booker = new User()
                },
                new Booth(){
                    Id = 2,
                    Booker = new User()
                }
            });

            int result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).CountAvailableBooths();

            Assert.Equal(0, result);
        }

        [Fact]
        public void testCountSingle()
        {
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
                new Booth(){
                    Id = 1,
                    Booker = new User()
                },
                new Booth(){
                    Id = 2,
                    Booker = new User()
                },
                new Booth(){
                    Id = 3,
                    Booker = new User()
                },
                new Booth(){
                    Id = 4,
                    Booker = null
                },
                new Booth(){
                    Id = 5,
                    Booker = new User()
                }
            });

            int result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).CountAvailableBooths();

            Assert.Equal(1, result);
        }

        [Fact]
        public void testCountAll()
        {
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Booth>
            {
                new Booth(){
                    Id = 1,
                    Booker = new User()
                },
                new Booth(){
                    Id = 2,
                    Booker = new User()
                },
                new Booth(){
                    Id = 3,
                    Booker = new User()
                },
                new Booth(){
                    Id = 4,
                    Booker = new User()
                },
                new Booth(){
                    Id = 5,
                    Booker = new User()
                }
            });

            int result = new BoothService(mockUserRepository.Object, mockBoothRepository.Object, mockAuthenticationService.Object, mockWaitingListRepository.Object).CountAvailableBooths();

            Assert.Equal(0, result);
        }
    }
}
