using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib.Models;
using RosterLib.Services;

namespace RosterLib.Tests
{
	[TestClass]
	public class NflVerseRosterMasterTests
	{
		private NflVerseRosterMaster _rosterMaster;

		[TestInitialize]
		public void Setup()
		{
			_rosterMaster = new NflVerseRosterMaster(
				"2025");
		}

		[TestMethod]
		public void ServiceInstantiatesOk()
		{
			Assert.IsNotNull(_rosterMaster);
		}

		[TestMethod]
		public void ServiceCanGetDepthChart()
		{
			var teamCode = "SF";
			var result = _rosterMaster.GetDepthChart(
				teamCode);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.PlayersOnRoster > 40);
			Assert.IsTrue(
				result.PlayersOnRoster < 70,
				$@"roster has too many players {
					result.PlayersOnRoster
					}");
			Console.WriteLine(
				$@"{
					teamCode
					} roster has {
					result.PlayersOnRoster
					} players");
		}

		[TestMethod]
		public void ServiceKnowsSfQb()
		{
			var result = _rosterMaster.PlayerAt(
				"SF",
				"QB",
				1);

			Assert.AreEqual(
				"Brock Purdy",
				result);
		}

		[TestMethod]
		public void ServiceKnowsSfQbPlayerInfo()
		{
			var result = _rosterMaster.PlayerInfoAt(
				"SF",
				"QB",
				1);

			Assert.IsInstanceOfType<PlayerInfo?>(
				result);

			Console.WriteLine(
				result.BirthDate);
			Console.WriteLine(
				result.College);
		}

		[TestMethod]
		public void ServiceKnowsSfQbPlayerInfoAt()
		{
			var result = _rosterMaster.PlayerInfoAt(
				"SF",
				"QB",
				2);

			Assert.IsInstanceOfType<PlayerInfo?>(
				result);

			Assert.AreEqual(
				"Mac Jones",
				result.FullName);

			Console.WriteLine(
				result.BirthDate);
			Console.WriteLine(
				result.College);
		}
	}
}
