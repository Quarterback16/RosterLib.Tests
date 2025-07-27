using RosterLib.Interfaces;

namespace RosterLib.Tests
{
	[TestClass]
	public class StatGridTests
	{
		private IKeepTheTime _timeKeeper;

		[TestInitialize]
		public void Initialize()
		{
			// This method is called before each test method runs.
			_timeKeeper = new TimeKeeper(clock: null);
			Console.WriteLine("Initializing StatGridTests...");
		}

		[TestMethod]
		public void TestStatGrid()
		{
			var sut = new StatGrid(
				_timeKeeper.PreviousSeason(), 
				"INTsThrown");

			var md = sut.Render();
			Console.WriteLine(md);
			Assert.IsTrue(File.Exists(sut.FileName()));
		}

		[TestMethod]
		public void TestStatGridSeasons()
		{
			var theSeason = new NflSeason("2024");
			theSeason.LoadRegularWeeksToDate();
			Assert.AreEqual(
				expected: _timeKeeper.WeeksInSeason(),
				actual: theSeason.RegularWeeks.Count);
		}

		[TestMethod]
		public void TestStatGridWeeksAllPassed()
		{
			var totalWeeks = 0;
			var theSeason = new NflSeason("2016");
			theSeason.LoadRegularWeeks();
			foreach (var week in theSeason.RegularWeeks)
			{
				if (!week.HasPassed())
				{
					Console.WriteLine($"{week.WeekKey()} missing");
					continue;
				}
				totalWeeks++;
			}
			Assert.AreEqual(expected: 17, actual: totalWeeks);
		}

		[TestMethod]
		public void TestStatGridWeekLogic()
		{
			var sut = new NFLWeek("2016", "14");
			Assert.IsTrue(sut.HasPassed());
		}

		[TestMethod]
		public void TestAllGamesHaveBeenPlayed()
		{
			var gamesPlayed = 0;
			var sut = new NFLWeek("2016", "14");
			var gList = sut.GameList();
			for (int i = 0; i < gList.Count; i++)
			{
				var g = (NFLGame)gList[i];
				if (g.Played())
					gamesPlayed++;
				else
					Console.WriteLine($"Game {g.GameName()}");
			}
			Assert.AreEqual(expected: 16, actual: gamesPlayed);
		}
	}
}
