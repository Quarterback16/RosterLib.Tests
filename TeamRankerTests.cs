﻿using RosterLib.Helpers;
using RosterLib.Implementations;
using RosterLib.Models;
using System.Data;
using System.Text;

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
		public void TeamRankerReturnsMetricsContext()
		{
			_sut.ForceReRank = true;
			var when = new DateTime(
					2025, 07, 17,
					0, 0, 0,
					DateTimeKind.Unspecified);

			var result = _sut.RankTeams(
				when);

			Assert.IsInstanceOfType(
				result, 
				typeof(MetricsContext));
			Assert.IsTrue(result.RankDate.Equals(
				new DateTime(
					2025, 09, 07,
					0, 0, 0,
					DateTimeKind.Unspecified)));
			Assert.IsTrue(result.RatingsHt.Count > 0);
			Assert.IsTrue(result.Data.Rows.Count > 0);

			DoRatingsSummary(result);

			var mi = new MarkdownInjector();
			var teamRank = 0;
			result.Data.DefaultView.Sort = "RPTS DESC";
			foreach (DataRowView row in result.Data.DefaultView)
			{
				teamRank++;
				var md =	MetricsContextHelper.TeamGradingsToMarkdown(
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

		private void DoRatingsSummary(MetricsContext metricsContext)
		{
			var summary = MetricsContextHelper.RankingSummary(
				metricsContext);
			Console.WriteLine(summary);
			Console.WriteLine(
				$@"Sending output to {
					MetricsContextHelper.GradingsSummaryFileName(metricsContext)}");
			File.WriteAllText(
				path: MetricsContextHelper.GradingsSummaryFileName(metricsContext),
				contents: summary);

		}	

		private string TeamPageFileName(
			string teamCode) => 

			 $"{Utility.CurrentSeason()}//Teams//{teamCode}.md";
		
	}
}
