using Core.Application;
using Core.Application.Implementation;
using Core.Domain;
using Core.Entity;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace XUnitTesting.BoothTest
{
    public class GetUnbookedBoothsTest
    {
        private Mock<IBoothRepository> mockBoothRepository = new Mock<IBoothRepository>();
        private IBoothService _boothServ;

        public GetUnbookedBoothsTest()
        {
            _boothServ = new BoothService(null, mockBoothRepository.Object, null, null);
        }

        /// <summary>
        /// Asserts whether the given booths are returned correctly if their booker is null.
        /// </summary>
        [Fact]
        public void AssertGetCorrectBoothsReturned()
        {
            //Users and booths are made
            #region
            User user = new User();
            Booth booth1 = new Booth()
            {
                Booker = user
            };
            Booth booth2 = new Booth()
            {
                Booker = null
            };
            Booth booth3 = new Booth()
            {
                Booker = null
            };
            #endregion
            List<Booth> list = new List<Booth>()
            {
                booth1,
                booth2,
                booth3
            };
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => list);

            Assert.True(_boothServ.GetUnbookedBooths().Count == 2);
            Assert.Contains(booth3, _boothServ.GetUnbookedBooths());
            Assert.Contains(booth2, _boothServ.GetUnbookedBooths());
        }

        /// <summary>
        /// Makes sure no booths are returned if there are no booths available
        /// </summary>
        [Fact]
        public void AssertEmptyListReturnsCorrently()
        {
            #region
            User user = new User();
            Booth booth1 = new Booth()
            {
                Booker = user
            };
            Booth booth2 = new Booth()
            {
                Booker = user
            };
            #endregion
            List<Booth> list = new List<Booth>()
            {
                booth1,
                booth2
            };
            mockBoothRepository.Setup(x => x.GetAll()).Returns(() => list);

            Assert.True(_boothServ.GetUnbookedBooths().Count == 0);
        }

    }
}
