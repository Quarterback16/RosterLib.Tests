using RosterLib.Helpers;
using RosterLib.Implementations;

namespace RosterLib.Tests
{
	[TestClass]
	public class ScheduleMasterTests
	{
		public ScheduleMaster Sut { get; set; }

		[TestInitialize]
		public void Setup()
		{
			Sut = new ScheduleMaster();
		}

		[TestMethod]
		public void ScheduleMaster_Instantiates_Ok()
		{
			Assert.IsNotNull(Sut);
		}

		[TestMethod]
		public void ScheduleMaster_KnowsYahooSchedule_Ok()
		{
			var game = Sut.GetGame(
				team: "7x7ers",
				leagueCode: "YAH",
				season: 2025,
				round: 04);
			Assert.IsNotNull(game);
			Assert.AreNotEqual("??", game.AwayTeam, "No Away team");
			Assert.AreNotEqual("??", game.HomeTeam, "No Home team");
			Console.WriteLine(game);
			Console.WriteLine(game.OpponentOf("77"));
			Console.WriteLine(
				CodeHelper.CodeFor(
					"YAH",
					game.OpponentOf("77")));
		}

		[TestMethod]
		public void ScheduleMaster_KnowsYahooOpponent_of77()
		{
			var opponent = Sut.OpponentOf(
				team: "7x7ers",
				leagueCode: "YAH",
				season: 2025,
				round: 03);
			Assert.IsNotNull(opponent);
			Assert.AreEqual("Finheads", opponent);
			Assert.AreEqual("FH", CodeHelper.CodeFor("YAH", opponent));

			Console.WriteLine(opponent);
		}

		[TestMethod]
		public void ScheduleMaster_KnowsYahooOpponent_of77_2025_02()
		{
			var opponent = Sut.OpponentOf(
				team: "7x7ers",
				leagueCode: "YAH",
				season: 2025,
				round: 02);
			Assert.IsNotNull(opponent);
			Assert.AreEqual("Super Hawks", opponent);
			Assert.AreEqual("SH", CodeHelper.CodeFor("YAH", opponent));

			Console.WriteLine(opponent);
		}

		[TestMethod]
		public void ScheduleMaster_HandlesNoGame_Ok()
		{
			var game = Sut.GetGame(
				team: "7x7ers",
				leagueCode: "YAH",
				season: 2023,
				round: 16);
			Assert.IsNull(game);
		}
	}
}
