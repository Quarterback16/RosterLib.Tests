using RosterLib.Implementations;

namespace RosterLib.Tests
{
	[TestClass]
	public class NflGameServiceTests
	{
		private NflGameService _sut;

		[TestInitialize]
		public void Setup()
		{
			_sut = new NflGameService(2024);
		}

		[TestMethod]
		public void NflGameService_CanLoadGames()
		{
			var games = _sut.GetAll();
			Assert.IsNotNull(games, "Games should not be null");
			Assert.IsTrue(games.Count > 0, "Games list should not be empty");
			Console.WriteLine($"Games Loaded: {games.Count}");
		}
	}
}
