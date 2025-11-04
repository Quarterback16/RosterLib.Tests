using System.Text.RegularExpressions;

namespace RosterLib.Tests
{
	[TestClass]
	public class PointsAllowedReportTests
	{
		[TestMethod]
		public void TestCurrentPointsAllowedReport()
		{
			var sut = new PointsAllowedReport(
				new TimeKeeper(null));
			sut.RenderAsHtml();
			Assert.IsTrue(
				File.Exists(sut.FileOut));
			Console.WriteLine(
				"{0} created.",
				sut.FileOut);
		}

		[TestMethod]
		public void TestReportSpecificWeek()
		{
			//  using this current week (not played yet) to generate `Points-Allowed-lw.json`
			//  need to do this in Playoffs as well
			var sut = new PointsAllowedReport(
				new FakeTimeKeeper(
					season: "2025",
					week: "09"));  //  next week
			sut.RenderAsHtml();
			Assert.IsTrue(File.Exists(sut.FileOut));
			Console.WriteLine("{0} created.", sut.FileOut);
		}

		[TestMethod]
		public void TestGettingRankFromString()
		{
			var theValue = "<a href='.//pts-allowed//NE-QB-01.htm'>31.02 (30)";
			var pattern = @"\((.*?)\)";
			var match = Regex.Match(theValue, pattern).Value;
			match = match.Replace('(', ' ');
			match = match.Replace(')', ' ');
			int rankNo = int.Parse(match);
			Assert.AreEqual(30, rankNo);
		}

		[TestMethod]
		public void TestReport()
		{
			for (int i = 11; i < 18; i++)
			{
				var sut = new PointsAllowedReport(
				   new FakeTimeKeeper(season: "2016", week: $"{i:0#}"));
				sut.RenderAsHtml();
				Assert.IsTrue(File.Exists(sut.FileOut));
				Console.WriteLine("{0} created.", sut.FileOut);
			}
		}

		[TestMethod]
		public void FractionOfTheSeasonAfter4weeksIs25Percent()
		{
			var sut = new PointsAllowedReport(
			   new FakeTimeKeeper(
				   season: "2017",
				   week: "04"));
			var fraction = sut.FractionOfTheSeason();
			Assert.AreEqual(3.0M / 17.0M, fraction);
		}

		[TestMethod]
		public void AfterFourWeeks100TotalPointsIsExcellent()
		{
			var sut = new PointsAllowedReport(
			   new FakeTimeKeeper(season: "2017", week: "04"));
			var colour = sut.TotBgPicker(100);
			Assert.AreEqual(Constants.Colour.Excellent, colour);
		}

		[TestMethod]
		public void AfterOneWeek_GivingUp_39_IsExcellent()
		{
			var sut = new PointsAllowedReport(
			   new FakeTimeKeeper(season: "2017", week: "01"));
			var colourExcellent = sut.TotBgPicker(39);
			Assert.AreEqual(
				Constants.Colour.Excellent,
				colourExcellent,
				"Excellent Colour wrong");
		}

		[TestMethod]
		public void AfterOneWeek_GivingUp_42_IsGood()
		{
			var sut = new PointsAllowedReport(
			   new FakeTimeKeeper(season: "2017", week: "01"));
			var colourGood = sut.TotBgPicker(42);
			Assert.AreEqual(
				Constants.Colour.Good,
				colourGood,
				"Good Colour wrong");
		}

		[TestMethod]
		public void AfterOneWeek_GivingUp_70_IsAverage()
		{
			var sut = new PointsAllowedReport(
			   new FakeTimeKeeper(season: "2017", week: "01"));
			var colourAverage = sut.TotBgPicker(70);
			Assert.AreEqual(
				Constants.Colour.Average,
				colourAverage,
				"Average Colour wrong");
		}

		[TestMethod]
		public void AfterOneWeek_GivingUp_85_IsBad()
		{
			var sut = new PointsAllowedReport(
			   new FakeTimeKeeper(season: "2017", week: "01"));
			var colourBad = sut.TotBgPicker(85);
			Assert.AreEqual(
				Constants.Colour.Bad,
				colourBad,
				"Bad Colour wrong");
		}

		[TestMethod]
		public void AfterOneWeek_GivingUp_14_toRBs_IsAverage()
		{
			var sut = new PointsAllowedReport(
			   new FakeTimeKeeper(season: "2017", week: "01"));
			var colour = sut.TotRbBgPicker(14);
			Assert.AreEqual(
				Constants.Colour.Average,
				colour,
				"average Colour wrong");
		}

		[TestMethod]
		public void TestAwayKickersLoadProperly()
		{
			var sut = new NFLGame("2017:01-A");

			var playerList = sut.LoadAllFantasyAwayPlayers(
				null,
				Constants.K_KICKER_CAT);

			Assert.AreEqual(1, playerList.Count);
		}

		[TestMethod]
		public void TestHomeQbsLoadProperly()
		{
			var sut = new NFLGame("2017:01-A");

			var playerList = sut.LoadAllFantasyHomePlayers(
				null,
				Constants.K_QUARTERBACK_CAT);

			Assert.AreEqual(2, playerList.Count);
		}

		[TestMethod]
		public void TestDallasGameWeek9()
		{
			var testSeason = "2018";
			var testWeek = "09";
			var ds = Utility.TflWs.GameForTeam(
				season: testSeason,
				week: testWeek,
				teamCode: "DC");
			Assert.IsTrue(ds.Tables[0].Rows.Count == 1);

			var game = new NFLGame(ds.Tables[0].Rows[0]);
			var playerList = game.LoadAllFantasyAwayPlayers(
					date: (DateTime?)game.GameDate,
					catFilter: String.Empty);
			Assert.IsTrue(playerList.Count > 0);
			var week = new NFLWeek(testSeason, testWeek);
			var scorer = new YahooXmlScorer(week);
			var qbCount = 0;
			var qbPts = 0.0M;
			foreach (var p in playerList)
			{
				if (p.IsQuarterback())
				{
					qbCount++;
					qbPts += scorer.RatePlayer(
						p,
						week,
						takeCache: false);
				}
			}
			Assert.IsTrue(qbCount > 0);
			Assert.IsTrue(qbPts > 0.0M);
		}

		[TestMethod]
		public void TestJson()
		{
			var sut = new PointsAllowedReport(
				new FakeTimeKeeper(season: "2022", week: "12"));
			sut.RenderAsJson();
			Assert.IsTrue(File.Exists(sut.JsonOut));
			Console.WriteLine("{0} created.", sut.JsonOut);
		}
	}
}
