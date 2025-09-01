namespace RosterLib.Tests
{
    [TestClass]
    public class DefensiveScorerTests
    {
        #region  Sut Initialisation

        private DefensiveScorer sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = SystemUnderTest();
        }

        private static DefensiveScorer SystemUnderTest() =>
            new DefensiveScorer(
                new TimeKeeper(clock:null));

        #endregion

        [TestMethod]
        public void DefensiveScorerInstantiatesOk()
        {
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void DefensiveScorerExecutesOk()
        {
            var result = sut.DoReport();
            Assert.IsNotNull(result);
        }

    }
}

