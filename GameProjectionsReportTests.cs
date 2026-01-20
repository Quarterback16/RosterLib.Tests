using RosterLib.Helpers;
using RosterLib.Implementations;

namespace RosterLib.Tests
{
	[TestClass]
	public class GameProjectionsReportTests
	{
		[TestMethod]
		public void TestGameProjectionsReport()
		{
			var cut = new GameProjectionsReport(
				new FakeTimeKeeper(
					"2023",
					"11"));
			Assert.IsNotNull(cut);
			cut.RenderAsHtml();
		}

		[TestMethod]
		public void TestGameProjectionsReportStructure()
		{
			var cut = new GameProjectionsReport(
				new FakeTimeKeeper(
					"2019",
					"12"));
			Assert.IsNotNull(cut);
			cut.RenderAsHtml(structOnly: true);
		}


		[TestMethod]
		public void TestSingleGameProjectionsReport()
		{
			var cut = new NFLGame("2025:21-A");
			Assert.IsNotNull(cut);
			var result = cut.WriteProjection(
				mi: new MarkdownInjector(
					FolderHelper.GetObsidianNflStemFolder()));
			Console.WriteLine(result);
		}
	}
}
