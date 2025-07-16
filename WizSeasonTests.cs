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
                "2024",
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

        [TestMethod]
        public void PredictionSeason_TalliesPredictions()
        {
            var ps = new PredictionSeason("2025");

            Assert.IsNotNull(ps);

            Console.WriteLine(
                $"Predicted TD passes {ps.TDp}");

            Console.WriteLine(
                $"Predicted YDp {ps.YDp}");

            Console.WriteLine(
                $"Predicted TD runs {ps.TDr}");

            Console.WriteLine(
                $"Predicted YDr {ps.YDr}");

            //sut.FrequencyTables(toMarkdown: true);
        }
    }
}
