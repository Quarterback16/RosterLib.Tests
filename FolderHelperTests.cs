using Helpers;

namespace RosterLib.Tests
{
	[TestClass]

	public class FolderHelperTests
	{
		[TestMethod]
		public void TestYahooXmlFolder()
		{
			var result = FolderHelper.YahooXmlFile();
			Console.WriteLine($"{Environment.MachineName} - {result}");
			if (Environment.MachineName == "MAHOMES")
				Assert.AreEqual(result, "c:/Users/quart/Dropbox/GridStat/XML/YahooOutput.xml");
			else
				Assert.AreEqual(result, "D:\\Dropbox\\GridStat\\XML\\YahooOutput.xml");
		}
	}
}
