using RosterLib.Helpers;
using RosterLib.Implementations;
using RosterLib.Services;
using TFLLib;

namespace RosterLib.Tests
{
    [TestClass]
    public class PlayerCsvTests
    {
        [TestMethod]
        public void TestPlayerCsvReportCanRenderToCsv()
        {
            var timeKeeper = new TimeKeeper(null);
            var sut = new PlayerCsv(
                timeKeeper,
                new AdpMaster(
                    $"{ConfigHelper.CsvFolder()}ADP {timeKeeper.CurrentSeason()}.csv"),
                new DoozyService(
                    timeKeeper.CurrentSeason(),
                    ConfigHelper.JsonFolder()),
                new ContractYearService(
                    timeKeeper.CurrentSeason(),
                    ConfigHelper.JsonFolder()),
                new ProjectionService(
                    new DbfPlayerGameMetricsDao(),
                    new DataLibrarian(
                        Utility.NflConnectionString(),
                        Utility.TflConnectionString(),
                        Utility.CtlConnectionString(),
                        logger: null)))
            {
                DoProjections = true  // 2024-06-01 decided to stick to one CSV format as it feeds into a lot of stuff
            };
            Assert.IsNotNull(sut);
            sut.DoReport();
        }

        [TestMethod]
        public void TestPlayerCsvReportCanRenderToMarkdown()
        {
            var timeKeeper = new TimeKeeper(null);
            var sut = new PlayerCsv(
                timeKeeper,
                new AdpMaster(
                    $"{ConfigHelper.CsvFolder()}ADP {timeKeeper.CurrentSeason()}.csv"),
                new DoozyService(
                    timeKeeper.CurrentSeason(),
                    ConfigHelper.JsonFolder()),
                new ContractYearService(
                    timeKeeper.CurrentSeason(),
                    ConfigHelper.JsonFolder()),
                new ProjectionService(
                    new DbfPlayerGameMetricsDao(),
                    new DataLibrarian(
                        Utility.NflConnectionString(),
                        Utility.TflConnectionString(),
                        Utility.CtlConnectionString(),
                        logger: null)))
            {
                DoProjections = true  // 2024-06-01 decided to stick to one CSV format as it feeds into a lot of stuff
            };
            Assert.IsNotNull(sut);
            sut.RenderAsMarkdown("QB");
        }

        [TestMethod]
        public void TestScoresPerYear()
        {
            var sut = new NFLPlayer("MANNPE01");
            var s = sut.ScoresPerYear();
            var testStr = s.ToString();
            var decSpot = testStr.IndexOf('.');
            var numDecPoints = testStr.Length - decSpot - 1;
            Assert.IsTrue(numDecPoints.Equals(2));
        }

        //  Test getting a players projections for a year
        [TestMethod]
        public void TestGetPlayerProjections()
        {
            var sut = new DbfPlayerGameMetricsDao();
            var pgms = sut.GetSeason("2022", "MAHOPA01");
            Assert.IsTrue(pgms.Count > 0);
            foreach (var item in pgms)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod]
        public void TestRatePlayerProjection()
        {
            var playerId = "MAHOPA01";
            var p = new NFLPlayer(playerId);
            var sut = new DbfPlayerGameMetricsDao();
            var pgms = sut.GetSeason("2022", playerId);
            var totalPoints = 0.0M;
            foreach (var pgm in pgms)
            {
                pgm.CalculateProjectedFantasyPoints(p);
                totalPoints += p.Points;
            }
            System.Console.WriteLine(
                $"{playerId} is projected for {totalPoints} FP");
            Assert.IsTrue(p.Points < 400M);
        }

        [TestMethod]
        public void TestGameCodeGet()
        {
            var sut = new NFLWeek("2014", 1);
            var gameCode = sut.GameCodeFor("DB");
            Assert.IsTrue(gameCode.Equals("2014:01-N"));
        }

        [TestMethod]
        public void TestPmetricsGet()
        {
            var player = new NFLPlayer("MANNPE01");
            var week = new NFLWeek("2014", 1);
            var gameCode = week.GameCodeFor("DB");
            var dao = new DbfPlayerGameMetricsDao();
            var pgm = dao.GetPlayerWeek(gameCode, player.PlayerCode);

            Assert.IsTrue(pgm.ProjYDp.Equals(300));
        }

        [TestMethod]
        public void TestGS4Scorer()
        {
            var player = new NFLPlayer("MANNPE01");
            var week = new NFLWeek("2014", 1);
            var sut = new GS4Scorer(week);
            var score = sut.RatePlayer(player, week);
            Assert.IsTrue(score.Equals(9.0M));
        }

        [TestMethod]
        public void TestYahooScorer()
        {
            var player = new NFLPlayer("MANNPE01");
            var week = new NFLWeek("2014", 1);
            var sut = new YahooScorer(week);
            var score = sut.RatePlayer(player, week);
            Assert.IsTrue(score.Equals(21.0M));
        }

        [TestMethod]
        public void TestYahooScorerLuck()
        {
            var player = new NFLPlayer("LUCKAN01");
            var week = new NFLWeek("2014", 14);
            var sut = new YahooScorer(week);
            var score = sut.RatePlayer(player, week);
            Assert.IsTrue(score.Equals(24.0M));
        }

        [TestMethod]
        public void TestYahooScorerLuckLastScores()
        {
            //  Luck ran one in in Week 14
            var plyr = new NFLPlayer("LUCKAN01");
            var ds = plyr.LastScores("R", 14, 14, "2014", "1");
            var nScores = ds.Tables[0].Rows.Count;
            Assert.IsTrue(nScores.Equals(1));
        }

        [TestMethod]
        public void TestAdpOut()
        {
            var sut = new RenderStatsToHtml(
                weekMasterIn: null);
            var result = sut.AsDraftRound(96);
            Assert.AreEqual("9.01", result);
        }

        [TestMethod]
        public void TestAdpOutNumber1()
        {
            var sut = new RenderStatsToHtml(null);
            var result = sut.AsDraftRound(1);
            Assert.AreEqual("1.01", result);
        }

        [TestMethod]
        public void TestLister()
        {
            var sut = new PlayerCsv(
                timekeeper: new TimeKeeper(null),
                adpMaster: null,
                dzService: null)
            {
                DoProjections = false,
                Configs = new List<StarterConfig>
                {
                    new StarterConfig
                    {
                       Category = Constants.K_RUNNINGBACK_CAT,
                       Position = "RB"
                    },
                }
            };
            sut.CollectPlayers();
            foreach (var item in sut.Lister.PlayerList)
            {
                Console.WriteLine(item);
            }
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void TestRBListerToMarkdown()
        {
            StartersProjections(
                "RB", 
                Constants.K_RUNNINGBACK_CAT);
        }

        [TestMethod]
        public void TestQBListerToMarkdown()
        {
            StartersProjections(
                "QB", 
                Constants.K_QUARTERBACK_CAT);
        }

        [TestMethod]
        public void TestWRListerToMarkdown()
        {
            StartersProjections("WR", Constants.K_RECEIVER_CAT);
        }

        [TestMethod]
        public void TestTEListerToMarkdown()
        {
            StartersProjections("TE", Constants.K_RECEIVER_CAT);
        }

        [TestMethod]
        public void TestListerToMarkdown()
        {
            StartersProjections("RB", Constants.K_RUNNINGBACK_CAT);
            StartersProjections("QB", Constants.K_QUARTERBACK_CAT);
            StartersProjections("WR", Constants.K_RECEIVER_CAT);
            StartersProjections("TE", Constants.K_RECEIVER_CAT);
        }

        private static void StartersProjections(
            string position,
            string category)
        {
            var sut = new PlayerCsv(
                timekeeper: new TimeKeeper(null),
                adpMaster: new AdpMaster(
                    ConfigHelper.PlayerCsvFile(
                        "2025")),
                dzService: null,
                startersOnly: true)
            {
                DoProjections = true,

                Configs = new List<StarterConfig>
                {
                    new StarterConfig
                    {
                       Category = category,
                       Position = position
                    },
                }
            };
            Assert.IsNotNull(sut);
            sut.RenderAsMarkdown(position);
        }
    }
}
