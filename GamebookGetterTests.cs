using Helpers;
using RosterLib.Implementations;
using RosterLib.Interfaces;

namespace RosterLib.Tests
{
	[TestClass]
	public class GamebookGetterTests
	{
		public string WeekToDownload { get; set; } = string.Empty;
		public IGetGamebooks? Sut { get; set; }
		public NFLWeek? Week { get; set; }

		[TestInitialize]
		public void Init()
		{
			WeekToDownload = new TimeKeeper(clock:null)
				.CurrentWeek()
				.ToString();

			Week = new NFLWeek(
				seasonIn: "2025",
				weekIn: WeekToDownload);

			string outputFolder;
			if (Utility.HostName().ToUpper() == "MAHOMES")
				outputFolder = $"c:\\tfl\\nfl\\gamebooks\\week {WeekToDownload}\\";
			else
				outputFolder = $"d:\\tfl\\nfl\\gamebooks\\week {WeekToDownload}\\";
			
			Console.WriteLine(
					$"Downloading gamebooks to {outputFolder}");
			Sut = new GamebookGetterViaGameId(
				new Downloader(
					outputFolder),
				new GameIdService());
		}

		[TestMethod]
		public void TestGetGamebooks()
		{
			var result = Sut?.DownloadWeek(Week);
			Assert.IsTrue(result > 0);
		}

		[TestMethod]
		public void TestNewDownloadSinglePdf()
		{
			var result = Sut?.Download(
				"2024",
				3,
				"patriots",
				"jets");
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void TestDownloadSinglePdf()
		{
			const string weekToDownload = "01";

			var sut = new Downloader(
			   $"e:\\tfl\\nfl\\gamebooks\\week {weekToDownload}\\");
			var uri = new Uri("http://www.nfl.com/liveupdate/gamecenter/57245/GB_Gamebook.pdf");
			var result = sut.Download(uri);
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void TestOutputDirectory()
		{
			var sut = new GamebookGetter(
				new Downloader("g:\\tfl\\nfl\\gamebooks\\week 03\\"));
			var result = sut.Downloader.OutputFolder;
			Assert.AreEqual("g:\\tfl\\nfl\\gamebooks\\week 03\\", result);
			Assert.IsTrue(System.IO.Directory.Exists(result));
		}

		[TestMethod]
		public void TestCurrentWeekSeed()
		{
			var week = new NFLWeek("2022", "01");
			var sut = new GamebookGetter(new Downloader());
			var result = sut.Seed(week);
			Assert.AreEqual("58838", result);
		}
	}
}
