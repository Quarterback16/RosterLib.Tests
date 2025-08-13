namespace RosterLib.Tests
{
	[TestClass]
	public class HotListTests
	{
		[TestMethod]
		public void TestDoHotlistsJob()  //  2015-11-08  1 min
		{
			var sut = new HotListReporter(
			new TimeKeeper(null));
			sut.Configs.Clear();
			sut.Configs.Add(new HotListConfig
			{
				Category = "3",
				Position = "WR",
				FreeAgents = true,
				Starters = true
			});

			var result = sut.DoReport();

			Assert.IsFalse(
				string.IsNullOrEmpty(
					result));

		}

	}
}
