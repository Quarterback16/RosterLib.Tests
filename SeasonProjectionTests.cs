namespace RosterLib.Tests
{
    [TestClass]
    public class SeasonProjectionTests
    {
        [TestMethod]
        public void TestNFLOutputMetric()
        {
            var t = new NflTeam("PE")
            {
                Season = "2025"
            };
            var predictor = new UnitPredictor
            {
                TakeActuals = true,
                AuditTrail = true,
                WriteProjection = false,
                StorePrediction = false,
                RatingsService = new UnitRatingsService(
                    new TimeKeeper(clock: null))
            };
            var sp = t.SeasonProjection(
                predictor,
                "Spread",
                new DateTime(2025, 7, 21, 0, 0, 0, DateTimeKind.Unspecified ));
            Assert.IsFalse(
                string.IsNullOrEmpty(sp));
        }

        [TestMethod]
        public void TestNflConferences()
        {
            var sut = new NflConference("AFC", "2017");
            sut.AddDiv("West", "H");
            Assert.IsTrue(sut.DivList.Count == 1);
        }

        [TestMethod]
        public void TestNflDivision()
        {
            var sut = new NFLDivision(
                nameIn: "West",
                confIn: "AFC",
                codeIn: "H",
                seasonIn: "2017",
                catIn: "");
            Assert.IsTrue(sut.TeamList.Count == 4);
        }

        [TestMethod]
        public void TestReportRunsOk()
        {
            var sut = new SeasonProjectionReport(
                new TimeKeeper(clock: null));
            var result = sut.DoReport();
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Console.WriteLine(result);
        }

    }
}
