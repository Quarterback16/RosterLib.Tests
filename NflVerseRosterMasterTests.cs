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
	}
}
