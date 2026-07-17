using RosterLib.Helpers;
using RosterLib.Implementations;
using RosterLib.Models;
using System.Data;

namespace RosterLib.Tests
{
	[TestClass]
	public class TeamRankerTests
	{
		#region  Sut Initialisation

		private TeamRanker? _sut;

		[TestInitialize]
		public void TestInitialize()
		{
			_sut = SystemUnderTest();
		}

		private static TeamRanker SystemUnderTest() =>
			new TeamRanker(
				new TimeKeeper(
					null));

		#endregion

		[TestMethod]
		public void TeamRankerInstantiatesOk()
		{
			Assert.IsNotNull(_sut);
		}

		[TestMethod]
		public void TeamRankerRanksTeams()
		{
			if (_sut != null) _sut.ForceReRank = true;
			var when = new DateTime(
					2026, 2, 4,
					0, 0, 0,
					DateTimeKind.Unspecified);

			var result = _sut?.RankTeams(
				when);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void TeamRankerCanTally_NE()
		{
			if (_sut != null)
				_sut.ForceReRank = true;
			var when = new DateTime(
					2026, 2, 4,
					0, 0, 0,
					DateTimeKind.Unspecified);

			_sut?.TallyTeam(
				new List<NflTeam>(),
				"2025",
				when,
				"NE");

			Assert.IsNotNull(when);
		}

		[TestMethod]
		public void TeamRankerReturnsMetricsContext()
		{
			if (_sut != null) _sut.ForceReRank = false;
			var when = new DateTime(
					2026, 06, 11,
					0, 0, 0,
					DateTimeKind.Unspecified);

			var rankings = _sut?.RankTeams(
				when);

			Assert.IsInstanceOfType(
				rankings,
				typeof(MetricsContext));
			Assert.IsTrue(rankings.RankDate.Equals(
				new DateTime(
					2026, 09, 13,
					0, 0, 0,
					DateTimeKind.Unspecified)));
			Assert.IsTrue(rankings.RatingsHt.Count > 0);

			DoRatingsSummary(rankings);

			if (rankings.Data != null)
			{
				var mi = new MarkdownInjector();
				var teamRank = 0;
				rankings.Data.DefaultView.Sort = "RPTS DESC";
				foreach (DataRowView row in rankings.Data.DefaultView)
				{
					teamRank++;
					var md = MetricsContextHelper.TeamGradingsToMarkdown(
						rankings,
						row,
						teamRank);
					Console.WriteLine(md);
					mi.InjectMarkdown(
						targetfile: TeamPageFileName(row["TEAM"].ToString()),
						tagName: "gradings",
						md);
					Console.WriteLine();
				}
			}

		}

		private static void DoRatingsSummary(
			MetricsContext metricsContext)
		{
			var summary = MetricsContextHelper.RankingSummary(
				metricsContext);
			metricsContext.Season = new TimeKeeper(null).CurrentSeason();
			Console.WriteLine(summary);
			Console.WriteLine(
				$@"Sending output to {MetricsContextHelper.GradingsSummaryFileName(
						metricsContext)}");
			File.WriteAllText(
				path: MetricsContextHelper.GradingsSummaryFileName(
					metricsContext),
				contents: summary);

		}

		private string TeamPageFileName(
			string teamCode) =>

			 $"{_sut?.TimeKeeper.CurrentSeason()}//Teams//{teamCode}.md";

		[TestMethod]
		public void UnitRankingsFromMetricsContextToMarkdown()
		{
			if (_sut != null) _sut.ForceReRank = true;
			var when = new DateTime(
					2026, 06, 11,
					0, 0, 0,
					DateTimeKind.Unspecified);

			var rankings = _sut?.RankTeams(
				when);

			var unitArray = UnitRatingsHelper.UnitArray();

			foreach (var unit in unitArray)
			{
				SendUnitGradingsToObsidian(
					unit,
					rankings);
			}

		}

		[TestMethod]
		public void POBreakdownsFromMetricsContextToMarkdown()
		{
			if (_sut != null) _sut.ForceReRank = true;
			var when = new DateTime(
					2026, 06, 11,
					0, 0, 0,
					DateTimeKind.Unspecified);
			_sut?.RankTeams(when);
			var md = MetricsContextHelper.BreakdownsToMarkdown(
				allContributions: _sut?.AllContributions,
				teamCode: "SF",
				unit: "PO",
				season: _sut?.TimeKeeper.CurrentSeason());

			Assert.IsFalse(string.IsNullOrEmpty(md));
			Console.WriteLine(md);
		}

		[TestMethod]
		public void AllBreakdownsToObsidian()
		{
			if (_sut != null) 
				_sut.ForceReRank = true;
			var when = new DateTime(
					2026, 06, 11,
					0, 0, 0,
					DateTimeKind.Unspecified);
			Assert.IsNotNull(_sut);
			_sut?.RankTeams(when);
			var teams = _sut?.AllContributions
					.GroupBy(ac => ac.Key)
					.Select(g => g.Key)
					.ToList();
			Assert.IsNotNull(teams);
			foreach (var team in teams)
			{
				var units = UnitRatingsHelper.UnitArray();
				foreach (var unit in units)
				{
					var md = MetricsContextHelper.BreakdownsToMarkdown(
						allContributions: _sut?.AllContributions,
						teamCode: team,
						unit: unit,
						season: _sut?.TimeKeeper.CurrentSeason());
					Assert.IsFalse(string.IsNullOrEmpty(md));
					SendBreakDownToObsidian(
						teamCode: team,
						unit: unit,
						season: _sut.TimeKeeper.CurrentSeason(),
						md: md);
				}
			}
		}

		private static void SendUnitGradingsToObsidian(
			string unit,
			MetricsContext? rankings)
		{
			var md = MetricsContextHelper.UnitRankingsToMarkDown(
				unit: unit,
				metricsContext: rankings);

			Assert.IsNotNull(md);
			Console.WriteLine(md);
			Console.WriteLine(
				$@"Sending output to {MetricsContextHelper.UnitGradingsFileName(
						unit, rankings)}");
			File.WriteAllText(
				path: MetricsContextHelper.UnitGradingsFileName(
					unit, rankings),
				contents: md);
		}

		private static void SendBreakDownToObsidian(
			string teamCode,
			string unit,
			string season,
			string md)
		{
			Assert.IsNotNull(md);
			Console.WriteLine(
				$@"Sending output to {MetricsContextHelper.BreakdownsFileName(
						teamCode, unit, season)}");
			Console.WriteLine(md);
			File.WriteAllText(
				path: MetricsContextHelper.BreakdownsFileName(
						teamCode,
						unit,
						season),
				contents: md);
		}
	}
}
