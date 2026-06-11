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
			_sut?.ForceReRank = true;
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
			_sut?.ForceReRank = true;
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
			_sut?.ForceReRank = false;
			var when = new DateTime(
					2026, 06, 11,
					0, 0, 0,
					DateTimeKind.Unspecified);

			var result = _sut?.RankTeams(
				when);

			Assert.IsInstanceOfType(
				result, 
				typeof(MetricsContext));
			Assert.IsTrue(result.RankDate.Equals(
				new DateTime(
					2026, 09, 13,
					0, 0, 0,
					DateTimeKind.Unspecified)));
			Assert.IsTrue(result.RatingsHt.Count > 0);

			DoRatingsSummary(result);

			if (result.Data != null)
			{
				var mi = new MarkdownInjector();
				var teamRank = 0;
				result.Data.DefaultView.Sort = "RPTS DESC";
				foreach (DataRowView row in result.Data.DefaultView)
				{
					teamRank++;
					var md = MetricsContextHelper.TeamGradingsToMarkdown(
						result,
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
				$@"Sending output to {
					MetricsContextHelper.GradingsSummaryFileName(
						metricsContext)}");
			File.WriteAllText(
				path: MetricsContextHelper.GradingsSummaryFileName(
					metricsContext),
				contents: summary);

		}	

		private string TeamPageFileName(
			string teamCode) => 

			 $"{_sut?.TimeKeeper.CurrentSeason()}//Teams//{teamCode}.md";
		
	}
}
