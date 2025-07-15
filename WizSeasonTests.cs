namespace RosterLib.Tests
{
    [TestClass]
    public class WizSeasonTests
    {
        #region  Sut Initialisation

        private WizSeason sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = SystemUnderTest();
        }

        private static WizSeason SystemUnderTest() =>
            new WizSeason(
                "2022",
                process: false);

        #endregion

        [TestMethod]
        public void WizSeasonInstantiatesOk()
        {
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void TestCurrentSeason()
        {
            sut.LoadSeason(
                seasonIn: "2024",
                startWeekIn: "01",
                endWeekIn: "18");

            sut.OutputPhase();
        }

        [TestMethod]
        public void WizSeason_HasFrequencyTables()
        {
            sut.LoadSeason(
                startWeekIn: "01",
                endWeekIn: "18");

            sut.FrequencyTables(toMarkdown: true);
        }
    }
}
