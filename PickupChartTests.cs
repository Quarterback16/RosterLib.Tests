using RosterLib.RosterGridReports;

namespace RosterLib.Tests
{
    [TestClass]
    public class PickupChartTests
    {
        [TestMethod]
        public void PickupChartRunsOk()
        {
            var sut = new PickupChart(
                new FakeTimeKeeper(season: "2025",week: "0"),
                week: 01);
            Assert.IsNotNull(sut);
            sut.RenderAsHtml();
        }

        [TestMethod]
        public void TestGamePlayedFlag()
        {
            var g = new NFLGame("2020:01-A");
            var hasBeenPlayed = g.Played(
                addDay: false);
            Assert.IsTrue(hasBeenPlayed);
        }

        [TestMethod]
        public void TestActualOutput()
        {
            var sut = new PickupChart(
                new FakeTimeKeeper(
                    season: "2020"),
                week: 01);
            var p = new NFLPlayer("MAHOPA01");
            var g = new NFLGame("2020:01-A");
            var result = sut.ActualOutput(
                game: g,
                player: p,
                runners: null);
            Assert.AreEqual(
                expected: " 34 ",
                actual: result);
        }

        [TestMethod]
        public void TestActualOutputCommittee()
        {
            var sut = new PickupChart(
                new FakeTimeKeeper(
                    season: "2020"),
                week: 01);
            var g = new NFLGame("2020:01-F");
            var runners = new List<NFLPlayer>
            {
                new NFLPlayer(
                    "INGRMA02"),
                new NFLPlayer(
                    "DOBBJ.01")
            };
            var result = sut.ActualOutput(
                game: g,
                player: null,
                runners: runners);
            Assert.AreEqual(
                expected: "17",
                actual: result);
        }

        [TestMethod]
        public void TestProjectedOutput()
        {
            var sut = new YahooCalculator();
            var p = new NFLPlayer("DAWSPH01");
            var g = new NFLGame("2017:10-A");
            var result = sut.Calculate(p, g);
            Assert.AreEqual(expected: 12, actual: p.Points);
        }

        [TestMethod]
        public void TestActualOutputDawson()
        {
            var sut = new PickupChart(
                new FakeTimeKeeper(season: "2017"), week: 10);
            var p = new NFLPlayer("DAWSPH01");
            var g = new NFLGame("2017:10-A");
            var result = sut.ActualOutput(g, p, null);
            Assert.AreEqual(expected: "  5 ", actual: result);
        }

        [TestMethod]
        public void TestGameHasBeenPlayed()
        {
            var g = new NFLGame("2020:02-A");
            Assert.IsTrue(g.Played());
        }

        [TestMethod]
        public void TestActualScoreInGameBit()
        {
            //var sut = new PickupChart(
            //    new FakeTimeKeeper(
            //        season: "2020"),
            //    week: 02);
            var g = new NFLGame("2020:02-A");
            //var t = new NflTeam("CL");
            var winner = new Winner
            {
                PointsFor = g.BookieTip.WinningScore(),
                Team = g.Team("CL"),
                Margin = Math.Abs(g.Spread),
                Home = g.IsHome("CL"),
                Game = g
            };
            var gamebit = PickupChart.GameBit(
                winner);

        }


        [TestMethod]
        public void TestJFournetteWeek1_2017()
        {
            var c = new YahooCalculator();
            var sut = new PickupChart(
                new FakeTimeKeeper(season: "2017"),
                week: 1);
            var p = new NFLPlayer("FOURLE01");
            var g = new NFLGame("2017:01-F");
            var result = sut.PlayerPiece(p, g, c);
            Console.WriteLine("Piece is {0}", result);
        }

        [TestMethod]
        public void TestLoadPassingUnit()
        {
            var sut = new NflTeam("KC");
            var passingUnit = sut.LoadPassUnit();
            Console.WriteLine("Passing Unit is {0}", passingUnit);
        }

        [TestMethod]
        public void TestLoadRushingUnit()
        {
            var sut = new NflTeam(
                "LR");
            var unit = sut.LoadRushUnit();
            Console.WriteLine("Rushing Unit is {0}", unit);
        }

        [TestMethod]
        public void TestPredictedResult()
        {
            var sut = new NFLGame("2014:16-G");
            var prediction = sut.PredictedResult();
            Assert.AreEqual("CP 20-CL 17", prediction);
        }

        [TestMethod]
        public void TestGamePlayed()
        {
            var sut = new NFLGame("2015:10-A");
            Assert.IsFalse(sut.Played());
        }

        [TestMethod]
        public void TestBookiePredictedResult()
        {
            var sut = new NFLGame("2016:04-O");
            sut.CalculateSpreadResult();
            var predictedResult = sut.BookieTip.PredictedScore();
            Assert.AreEqual("MV*24vNG*20", predictedResult);
        }

        [TestMethod]
        public void TestBookiePredictedResultPickEmGame()
        {
            var sut = new NFLGame("2015:11-B");
            sut.CalculateSpreadResult();
            var predictedResult = sut.BookieTip.PredictedScore();
            Assert.AreEqual("DL 25vOR 24", predictedResult);
        }

        [TestMethod]
        public void TestMarginOfVictory()
        {
            var sut = new NFLGame("2014:16-H");
            sut.CalculateSpreadResult();
            var marginOfVictory = sut.MarginOfVictory();
            Assert.AreEqual(7, marginOfVictory);
        }

        [TestMethod]
        public void TestFavouriteTeam()
        {
            var sut = new NFLGame("2014:16-H");
            sut.CalculateSpreadResult();
            Assert.AreEqual("NO", sut.FavouriteTeam().TeamCode);
        }

        [TestMethod]
        public void TestPlayerNameTo()
        {
            var sut = new NFLPlayer("ROETBE01");
            var sn = sut.PlayerNameTo(10);
            Assert.AreEqual("BRoethlisb", sn);
        }

        [TestMethod]
        public void TestKicker()
        {
            var sut = new NflTeam("SF");
            sut.LoadKickUnit();
            Assert.IsNotNull(sut.KickUnit);
            Assert.IsNotNull(sut.KickUnit.PlaceKicker);
        }

        [TestMethod]
        public void TestKickerCalcs()
        {
            var sut = new YahooCalculator();
            var p = new NFLPlayer("GOSTST01");
            var g = new NFLGame("2015:09-C");
            sut.Calculate(p, g);
            Assert.IsTrue(p.Points > 1);
        }

        [TestMethod]
        public void TestProjectedPoints()
        {
            var sut = new YahooCalculator();
            var p = new NFLPlayer("DAWSPH01");
            var g = new NFLGame("2017:10-A");
            sut.Calculate(p, g);
            Assert.IsTrue(p.Points == 12);
        }

        [TestMethod]
        public void TestW2Bit()
        {
            var sut = new PickupChart(
                new FakeTimeKeeper(season: "2022"),
                week: 1);
            var g = new NFLGame("2022:01-L");
            var team = new Winner
            {
                Team = g.Team("KC"),
                Margin = Math.Abs(g.Spread),
                Home = g.IsHome("KC"),
                Game = g
            };
            team.Team.LoadPassUnit();
            team.Team.PassUnit.SetReceiverRoles();
            var bit = sut.GetPlayerBit(
                team.Team.PassUnit.W2,
                team,
                new YahooCalculator());
            Assert.AreEqual(
                "MValdes",
                team.Team.PassUnit.W2.PlayerNameShort);
        }

        [TestMethod]
        public void TestPlayerPiece()
        {
            var c = new YahooCalculator();
            var sut = new PickupChart(
                new FakeTimeKeeper(season: "2024"), week: 1);
            var p = new NFLPlayer("EDWAGU01");
            var g = new NFLGame("2024:01-L");
            var result = sut.PlayerPiece(p, g, c);
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Console.WriteLine("Piece is {0}", result);
        }

        [TestMethod]
        public void TestRunnerBit()
        {
            var sut = new PickupChart(
                new FakeTimeKeeper(
                    season: "2022"),
                week: 1);
            var g = new NFLGame("2022:01-L");
            var team = new Winner
            {
                Team = g.Team("KC"),
                Margin = Math.Abs(g.Spread),
                Home = g.IsHome("AC"),
                Game = g
            };
            team.Team.LoadPassUnit();
            team.Team.PassUnit.SetReceiverRoles();
            team.Team.LoadRushUnit();
            var bit = sut.GetRunnerBit(
                team,
                new YahooCalculator());
            Assert.IsFalse(string.IsNullOrEmpty(bit));
            Console.WriteLine(bit);
        }

        [TestMethod]
        public void TestRunnerBit_Dual()
        {
            var sut = new PickupChart(
                new FakeTimeKeeper(
                    season: "2020"),
                    week: 1);
            var g = new NFLGame("2020:01-F");
            var team = new Winner
            {
                Team = g.Team("BR"),
                Margin = Math.Abs(g.Spread),
                Home = g.IsHome("BR"),
                Game = g
            };
            team.Team.LoadPassUnit();
            team.Team.PassUnit.SetReceiverRoles();
            team.Team.LoadRushUnit();
            var bit = sut.GetRunnerBit(
                team,
                new YahooCalculator());
            Console.WriteLine(bit);
            Assert.IsTrue(bit.Length == 121);
        }

        [TestMethod]
        public void TestOwners()
        {
            var p = new NFLPlayer("MAHOPA01");
            var result = p.LoadAllOwners();
            Assert.AreEqual("CC  77  TS", result);
        }

        [TestMethod]
        public void TestLine()
        {
            var week = new NFLWeek("2024", "12");
            Assert.IsNotNull(week);
            foreach (NFLGame game in week.GameList())
            {
                Console.WriteLine($"{game.GameName(),-23} {game.Spread}");
            }
        }
    }
}
