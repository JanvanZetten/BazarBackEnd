using System;
using System.Collections.Generic;
using Core.Application.Implementation;
using Core.Domain;
using Moq;
using Xunit;

namespace XUnitTesting.Booth
{
    public class CountAvalibleBoothsTest
    {
        private Mock<IRepository<Core.Entity.Booth>> mockBoothRepository = new Mock<IRepository<Core.Entity.Booth>>();

        [Fact]
        public void testCount(){
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Core.Entity.Booth>
            {
                new Core.Entity.Booth(){
                    Id = 1,
                    Booker = null
                },
                new Core.Entity.Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 3,
                    Booker = null
                },
                new Core.Entity.Booth(){
                    Id = 4,
                    Booker = null
                },
                new Core.Entity.Booth(){
                    Id = 5,
                    Booker = new Core.Entity.User()
                }
            });

            int result = new BoothService(mockBoothRepository.Object).CountAvalibleBooths();

            Assert.Equal(3, result);
        }

        [Fact]
        public void testCountNone()
        {
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Core.Entity.Booth>
            {
                new Core.Entity.Booth(){
                    Id = 1,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User()
                }
            });

            int result = new BoothService(mockBoothRepository.Object).CountAvalibleBooths();

            Assert.Equal(0, result);
        }

        [Fact]
        public void testCountSingle()
        {
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Core.Entity.Booth>
            {
                new Core.Entity.Booth(){
                    Id = 1,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 3,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 4,
                    Booker = null
                },
                new Core.Entity.Booth(){
                    Id = 5,
                    Booker = new Core.Entity.User()
                }
            });

            int result = new BoothService(mockBoothRepository.Object).CountAvalibleBooths();

            Assert.Equal(1, result);
        }

        [Fact]
        public void testCountAll()
        {
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => new List<Core.Entity.Booth>
            {
                new Core.Entity.Booth(){
                    Id = 1,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 2,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 3,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 4,
                    Booker = new Core.Entity.User()
                },
                new Core.Entity.Booth(){
                    Id = 5,
                    Booker = new Core.Entity.User()
                }
            });

            int result = new BoothService(mockBoothRepository.Object).CountAvalibleBooths();

            Assert.Equal(0, result);
        }

    }
}
