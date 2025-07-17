using RosterLib.Models;

namespace RosterLib.Tests
{
    [TestClass]
    public class TeamRankerTests
    {
        #region  Sut Initialisation

        private TeamRanker? _sut;

        [TestInitialize]
        public void TestInitialize()
        {
            _sut = SystemUnderTest();
        }

        private static TeamRanker SystemUnderTest() =>
            new TeamRanker(
                new FakeTimeKeeper(
                    "2025",
                    "0"));

        #endregion

        [TestMethod]
        public void TeamRankerInstantiatesOk()
        {
            Assert.IsNotNull(_sut);
        }

        [TestMethod]
        public void TeamRankerReturnsMetricsContext()
        {
            Utility.TflWs.UseCache = true;
            Assert.IsTrue(Utility.TflWs.UseCache);
            var ds = Utility.TflWs.TeamsDs("2025");
            _sut.ForceReRank = true;
            var when = new DateTime(
                    2025, 07, 17,
                    0, 0, 0,
                    DateTimeKind.Unspecified);

            var result = _sut?.RankTeams(
                when);
            Assert.IsInstanceOfType(
                result, 
                typeof(MetricsContext));
            Assert.IsTrue(result.RankDate.Equals(
                new DateTime(
                    2025,09,04,
                    0,0,0,
                    DateTimeKind.Unspecified)));
            Assert.IsTrue(result.RatingsHt.Count > 0);
            Assert.IsTrue(result.Data.Rows.Count > 0);
        }

    }
}
