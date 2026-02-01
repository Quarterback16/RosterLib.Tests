namespace RosterLib.Tests
{
    [TestClass]
    public class YahooCalculatorTests
    {
        [TestMethod]
        public void TestYahooProjectedPointsForLataviusMurray2016Week01()
        {
            var p = new NFLPlayer("MURRLA01");
            var g = new NFLGame("2016:01-F");
            g.LoadPrediction();
            var c = new YahooCalculator();
            var msg = c.Calculate(p, g);
            var expected = 17.2M;  //  125(1)
            Assert.AreEqual(expected, msg.Player.Points);
        }

        [TestMethod]
        public void TestYahooProjectedPointsForPeytonManning2014Week01()
        {
            var p = new NFLPlayer("NEWTCA01");
            var g = new NFLGame("2015:08-N");
            g.LoadPrediction();
            var c = new YahooCalculator();
            var msg = c.Calculate(p, g);
            var expected = 6;  //  0 TDp and 150 YDp
            Assert.AreEqual(expected, msg.Player.Points);
        }

        [TestMethod]
        public void TestYahooProjectedPointsFrTomBrady2013Week01()
        {
            var p = new NFLPlayer("BRADTO01");
            var g = new NFLGame("2013:01-B");
            g.LoadPrediction();
            var c = new YahooCalculator();
            var msg = c.Calculate(p, g);
            var expected = 18;  //  2 TDp and 250 YDp
            Assert.AreEqual(expected, msg.Player.Points);
        }

        [TestMethod]
        public void TestYahooProjectedPointsForSammyWatkinsWeek16()
        {
            var p = new NFLPlayer("WATKSA01");
            var g = new NFLGame("2014:16-M");
            g.LoadPrediction();
            var c = new YahooCalculator();
            var msg = c.Calculate(p, g);
            var expected = 13;  //  1 TDp and 75 YDc
            Assert.AreEqual(expected, msg.Player.Points);
        }

        [TestMethod]
        public void TestYahooMaster()
        {
            var sut = new YahooMasterGenerator(
                fullSeason: false,
                timekeeper: new FakeTimeKeeper(
                    season: "2016", 
                    week: "09"));
            var key = $"{"2016"}:{"09"}:{"CARRDE01"}";
            var stat = sut.YahooMaster.TheHt[key];
            Assert.IsNotNull(stat);
        }

        [TestMethod]
        public void TestYahooProjectedPointsForRJHarvey()
        {
            var playerProjection = new PlayerProjection(
                playerId: "HARVRJ01",
                season: "2025", 
                week: 4);
            var p = new NFLPlayer("HARVRJ01");
            var g = new NFLGame("2025:13-O");
            g.LoadPrediction();
            var c = new YahooCalculator();
            var msg = c.Calculate(p, g);
            var expected = 4.6M;  //  125(1)
            Assert.AreEqual(expected, msg.Player.Points);
            var gp = playerProjection.GameProjection(
                    g,
                    p.CurrTeam.TeamCode,
                    p.PlayerCat,
                    p.PlayerRole);
            Console.WriteLine($"hTDR: {g.ProjectedHomeTdr}");
        }

        [TestMethod]
        public void YahooGeneratorShouldOnlyGenerateForRegSeasonGames()
        {
            var sut = new YahooMasterGenerator(
                fullSeason: false,
                timekeeper: new FakeTimeKeeper(
                    season: "2025",
                    week: "20"));

            sut.RenderAsHtml();



        }

    }
}
