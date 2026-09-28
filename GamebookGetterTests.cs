using Helpers;
using RosterLib.Implementations;
using RosterLib.Interfaces;

namespace RosterLib.Tests
{
	[TestClass]
	public class GamebookGetterTests
	{
		public int WeekToDownload { get; set; }
		public IGetGamebooks? Sut { get; set; }
		public NFLWeek? Week { get; set; }

		[TestInitialize]
		public void Init()
		{
			var tk = new TimeKeeper(clock: null);

			if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday
				|| DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
				WeekToDownload = Int32.Parse(
					tk.PreviousWeek());
			else
				WeekToDownload = tk
					.CurrentWeek();

			Week = new NFLWeek(
				seasonIn: tk.CurrentSeason(),
				weekIn: WeekToDownload);

			Sut = new GamebookGetterViaGameId(
				downloader: new Downloader(
					outputFolder: SetOutputFolder()),
				gs: new GameIdService());
		}

		private string SetOutputFolder()
		{
			var outputFolder = $"d:\\tfl\\nfl\\gamebooks\\week {WeekToDownload:0#}\\";
			if (Utility.HostName().ToUpper() == "MAHOMES")
				outputFolder = $"c:\\tfl\\nfl\\gamebooks\\week {WeekToDownload:0#}\\";
			Console.WriteLine(
					$"Downloading Week {WeekToDownload} gamebooks to {outputFolder}");
			return outputFolder;
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
				"2026",
				1,
				"bills",
				"texans");
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