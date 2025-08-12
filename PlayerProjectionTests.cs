using RosterLib.Helpers;
using RosterLib.Implementations;
using System.Configuration;

namespace RosterLib.Tests
{
	[TestClass]
	public sealed class PlayerProjectionTests
	{
		[TestMethod]
		public void PlayerProjectionRendersHtml()
		{
			var pp = new PlayerProjection(
				playerId: "SAMUDE01",
				season: "2025");
			pp.Render();
			var fileOut = pp.FileName();
			Assert.IsTrue(
				File.Exists(fileOut),
				$"Cannot find {fileOut}");

			var md = MetricsHelper.ToMarkdown(
				 pp.TotalPlayerGameMetrics,
				 pp.Player);
			Console.WriteLine(md);
		}

		[TestMethod]
		public void AppSettings_AreLoaded()
		{
			string[] keys = 
			{
				"ObsidianFolder"
			};

			foreach (var key in keys)
			{
				var value = ConfigurationManager.AppSettings[key];
				Assert.IsFalse(
					string.IsNullOrEmpty(value),
					$"AppSetting '{key}' is missing or empty.");
			}
		}

		[TestMethod]
		public void ConnectionStrings_AreLoaded()
		{
			var nfl = ConfigurationManager.ConnectionStrings["NflConnectionString"]?.ConnectionString;
			var tfl = ConfigurationManager.ConnectionStrings["TflConnectionString"]?.ConnectionString;
			var ctl = ConfigurationManager.ConnectionStrings["CtlConnectionString"]?.ConnectionString;

			Assert.IsFalse(string.IsNullOrEmpty(nfl), "NflConnectionString is missing or empty.");
			Assert.IsFalse(string.IsNullOrEmpty(tfl), "TflConnectionString is missing or empty.");
			Assert.IsFalse(string.IsNullOrEmpty(ctl), "CtlConnectionString is missing or empty.");
		}

		[TestMethod]
		public void PlayerProjectionExposesSimpleTableReport()
		{
			var pp = new PlayerProjection(
				playerId: "ALLEJO02",
				season: "2025");
			pp.Render(linked: true);
			var str = pp.Str;
			Assert.IsNotNull(
				str,
				"a STR should be available");
			var md = SimpleTableReportHelper.ToMarkdown(
				str);
			Console.WriteLine(md);
		}

		[TestMethod]
		public void PlayerProjectionInjectsIntoObsidian()
		{
			var player = new NFLPlayer(
				playerId: "MIXOJO01");
			PlayerProjectionHelper.InjectProjection(
				player,
				"2025",
				new MarkdownInjector(
					FolderHelper.PlayerMarkdownFolder()));
			Assert.IsNotNull(player);
 
		}

		[TestMethod]
		public void PlayerProjectionCreatesPlayerFile()
		{
			var player = new NFLPlayer(
				playerId: "GOREDE01");
			var result = PlayerProjectionHelper.InjectProjection(
				player,
				"2025",
				new MarkdownInjector(
					FolderHelper.PlayerMarkdownFolder()));
			Assert.IsTrue(string.IsNullOrEmpty(result));
		}

		[TestMethod]
		public void PlayerProjection_KnowIfTagExists()
		{
			var folder = FolderHelper.PlayerMarkdownFolder();
			Assert.IsFalse(
				string.IsNullOrEmpty(folder), 
				"PlayerMarkdownFolder returned null or empty.");

			var host = Utility.HostName();
			if (host == "MAHOMES")
			{
				StringAssert.StartsWith(folder, @"C:\Users\quart\Dropbox\Obsidian\ChestOfNotes\01 - nfl\players\");
			}
			else
			{
				// The expected prefix depends on your app.config/appsettings for ObsidianFolder
				var obsidianFolder = FolderHelper.ObsidianFolder();
				StringAssert.StartsWith(
					folder, 
					obsidianFolder + "01 - nfl//players//");
			}
		}

		[TestMethod]
		public void MarkdownInjectorKnowsIfTagExists()
		{
			var mi = new MarkdownInjector(
					FolderHelper.PlayerMarkdownFolder());
			Assert.IsTrue(
				mi.FileContainsTag(
					PlayerProjectionHelper.PlayerMarkdownFile(
						new NFLPlayer("ALLEJO02")),
					"projection-2025"));
		}

		[TestMethod]
		public void PlayerProjectionInsertsTagsIfMissing()
		{
			var player = new NFLPlayer(
				playerId: "COOKDA01");
			var result = PlayerProjectionHelper.InjectProjection(
				player,
				"2025",
				new MarkdownInjector(
					FolderHelper.PlayerMarkdownFolder()));
			Assert.IsTrue(string.IsNullOrEmpty(result));
		}

		//[Ignore]  //  its a slow integration test
		[TestMethod]
		public void TestPlayerProjection()
		{
			var playerId = "SAMUDE01";
			var g = new NFLGame("2025:01-J");
			var testMsg = new PlayerGameProjectionMessage()
			{
				Player = new NFLPlayer(playerId),
				Game = g,
				Prediction = g.GetPrediction("unit")
			};
			var cut = new PullMetricsFromPrediction(testMsg);
			Assert.IsTrue(
				testMsg.Game.PlayerGameMetrics.Count > 12);
			foreach (var pgm in testMsg.Game.PlayerGameMetrics)
			{
				if (pgm.PlayerId == playerId)
				{
					Console.WriteLine(pgm);
					Assert.IsTrue(pgm.ProjYDc > 0);
				}
			}
		}

		[TestMethod]
		public void TestWeeklyProjection()
		{
			var w = new NFLWeek("2014", "06");
			var sut = new PlayerProjectionGenerator(
				new FakeTimeKeeper(
					season: "2014",
					week: "06"), null);
			var nGames = 0;
			for (var i = 0; i < w.GameList().Count; i++)
			{
				var game = (NFLGame)w.GameList()[i];
				sut.Execute(game);
				nGames++;
			}
			Assert.IsTrue(nGames > 10 && nGames < 17);
		}

		[TestMethod]
		public void TestProjectionOneGame()
		{
			var g = new NFLGame
				("2025:01-A");
			var sut = new PlayerProjectionGenerator(
				new FakeTimeKeeper(
					season: "2025", 
					week: "00"), 
				playerCache: null);
			var result = sut.Execute(g);
			Assert.IsFalse(
				string.IsNullOrEmpty(result));
			Console.WriteLine(result);
		}
	}
}
