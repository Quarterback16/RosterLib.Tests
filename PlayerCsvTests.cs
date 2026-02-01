using RosterLib.Helpers;
using RosterLib.Implementations;
using RosterLib.Interfaces;
using RosterLib.Services;
using System.Data;
using System.Text;
using TFLLib;

namespace RosterLib.Tests
{
	[TestClass]
	public class PlayerCsvTests
	{
		public TimeKeeper? _tk;
		public PlayerCsv? _sut;

		[TestInitialize]
		public void Setup()
		{
			_tk = new TimeKeeper(clock: null);
			var adpCsvFile = ConfigHelper.AdpCsvFile(
				season: _tk.CurrentSeason());
			_sut = new PlayerCsv(
				timekeeper: _tk,
				adpMaster: new AdpMaster(adpCsvFile),
				dzService: null,
				startersOnly: true)
			{
				DoProjections = true,
			};
		}

		[TestMethod]
		public void PlayerCsvReportCreatesCsvFile()
		{
			var timeKeeper = new TimeKeeper(null);
			var sut = new PlayerCsv(
				timeKeeper,
				new AdpMaster(2025),
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
				DoProjections = false  // 2024-06-01 decided to stick to one CSV format as it feeds into a lot of stuff
			};
			Console.WriteLine(
				$"Using Data from {Utility.NflConnectionString()}");
			Assert.IsNotNull(sut);
			sut.RenderAsHtml();
			Assert.IsTrue(
				File.Exists(
					ConfigHelper.PlayerCsvFile(timeKeeper.CurrentSeason())),
				"CSV file was not created.");
		}


		[TestMethod]
		public void TestPlayerCsvReportCanRenderToMarkdown()
		{
			var timeKeeper = new TimeKeeper(null);
			var sut = new PlayerCsv(
				timeKeeper,
				new AdpMaster(
					Int32.Parse(timeKeeper.CurrentSeason())),
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
			sut.Configs = new List<StarterConfig>
			{
				new StarterConfig
				{
					Category = Constants.K_RECEIVER_CAT,
					Position = "TE"
				},
			};
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
			var md = _sut?.StartersProjections(
				"RB", 
				Constants.K_RUNNINGBACK_CAT);
			Assert.IsFalse(
				string.IsNullOrEmpty(md), 
				"Markdown output is empty.");
			Console.WriteLine(md);
		}

		[TestMethod]
		public void TestQBListerToMarkdown()
		{
			var md = _sut?.StartersProjections(
				"QB", 
				Constants.K_QUARTERBACK_CAT);
			Assert.IsFalse(
				string.IsNullOrEmpty(md),
				"Markdown output is empty.");
			Console.WriteLine(md);
		}

		[TestMethod]
		public void TestWRListerToMarkdown()
		{
			var md = _sut?.StartersProjections(
				"WR", 
				Constants.K_RECEIVER_CAT);
			Assert.IsFalse(
				string.IsNullOrEmpty(md),
				"Markdown output is empty.");
			Console.WriteLine(md);
		}

		[TestMethod]
		public void TestTEListerToMarkdown()
		{
			var md = _sut?.StartersProjections(
				"TE", 
				Constants.K_RECEIVER_CAT);
			Assert.IsFalse(
				string.IsNullOrEmpty(md),
				"Markdown output is empty.");
			Console.WriteLine(md);
		}

		[TestMethod]
		public void TestAllPositionsToMarkdown()
		{
			var output = new StringBuilder()
				.AppendLine(
					_sut?.StartersProjections(
						"RB", 
						Constants.K_RUNNINGBACK_CAT))
				.AppendLine(
					_sut?.StartersProjections(
						"QB", 
						Constants.K_QUARTERBACK_CAT))
				.AppendLine(
					_sut?.StartersProjections(
						"WR", 
						Constants.K_RECEIVER_CAT))
				.AppendLine(
					_sut?.StartersProjections(
						"TE", 
						Constants.K_RECEIVER_CAT))
				.ToString();
			Assert.IsFalse(string.IsNullOrEmpty(output));
		}

		[TestMethod]
		public void TestFP18Missing()
		{
			var cut =  new RenderStatsToHtml(
				weekMasterIn: null);
			cut.Season = "2025";
			cut.Week = 18;
            cut.WeeksToGoBack = 17;

            var dt = new DataTable();
            cut.DefinePlayerCsvColumns(dt);
            var dr = dt.NewRow();
            var result = cut.AddWeeklyFp(
				new NFLPlayer("MCCACH03"),
				dr,
				new YahooStatService());
			Assert.IsTrue(
				result[17]>0M);
        }
    }
}
