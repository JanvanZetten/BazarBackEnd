using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class BoothServiceBookTest
    {
        readonly IBoothService _boothService;

        private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private Mock<IRepository<Booth>> mockBoothRepository = new Mock<IRepository<Booth>>();

        private Dictionary<int, User> userDictionary = new Dictionary<int, User>();
        private Dictionary<int, Booth> boothDictionary = new Dictionary<int, Booth>();

        private User user1;
        private User user2;

        private Booth booth1;
        private Booth booth2;

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
                Id = 1,
                Booker = user1
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

            _boothService = new BoothService(mockBoothRepository.Object);
        }

        /// <summary>
        /// Make valid booking.
        /// </summary>
        [Fact]
        public void BookValidInput()
        {

        }
    }
}
