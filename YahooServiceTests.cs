using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib.Services;
using System.IO;
using System.Linq;

namespace RosterLib.Tests
{
	[TestClass]
	public class YahooServiceTests
	{
		private const string TestXml = @"<stat-list>
  <stat id=""TESTQB01"" season=""2025"" week=""01"" qty=""12.5"" />
  <stat id=""TESTQB01"" season=""2025"" week=""02"" qty=""18"" />
  <stat id=""TESTWR01"" season=""2025"" week=""01"" qty=""21"" />
  <stat id=""TESTQB01"" season=""2024"" week=""01"" qty=""9.5"" />
  <stat id=""TESTQB01"" season=""2025"" week=""01"" qty=""99"" />
</stat-list>";

		private string _xmlFile = null!;
		private YahooStatService _service = null!;

		[TestInitialize]
		public void SetUp()
		{
			_xmlFile = Path.GetTempFileName();
			File.WriteAllText(_xmlFile, TestXml);
			_service = new YahooStatService(_xmlFile);
		}

		[TestCleanup]
		public void TearDown()
		{
			if (File.Exists(_xmlFile))
				File.Delete(_xmlFile);
		}

		[TestMethod]
		public void ConstructorShouldUseProvidedXmlFile()
		{
			Assert.AreEqual(_xmlFile, _service.XmlFile);
			Assert.IsNotNull(_service.Xdoc);
			Assert.AreEqual("stat-list", _service.Xdoc.Root?.Name.LocalName);
		}

		[TestMethod]
		public void GetStatShouldReturnFirstMatchingQuantity()
		{
			var result = _service.GetStat(
				playerId: "TESTQB01",
				season: "2025",
				week: "01");

			Assert.AreEqual(12.5M, result);
			Assert.AreEqual(18M, _service.GetStat("TESTQB01", "2025", "02"));
			Assert.AreEqual(9.5M, _service.GetStat("TESTQB01", "2024", "01"));
		}

		[TestMethod]
		public void GetStatShouldReturnZeroWhenNoMatchExists()
		{
			Assert.AreEqual(
				0M,
				_service.GetStat("NOPLAYER", "2025", "01"));

			Assert.AreEqual(
				0M,
				_service.GetStat("TESTQB01", "2025", "03"));
		}

		[TestMethod]
		public void IsStatShouldOnlyReturnTrueForKnownKey()
		{
			Assert.IsTrue(_service.IsStat("TESTQB01", "2025", "01"));
			Assert.IsTrue(_service.IsStat("TESTWR01", "2025", "01"));

			Assert.IsFalse(_service.IsStat("TESTQB01", "2025", "03"));
			Assert.IsFalse(_service.IsStat("NOPLAYER", "2025", "01"));
		}

		[TestMethod]
		public void LoadStatsShouldReturnAllRowsForPlayerSeasonAndWeek()
		{
			var results = _service.LoadStats("TESTQB01", "2025", "01").ToList();

			Assert.AreEqual(2, results.Count);
			CollectionAssert.AreEqual(
				new[] { 12.5M, 99M },
				results.Select(s => s.Qty).ToArray());
		}

		[TestMethod]
		public void LoadStatsShouldReturnEmptyWhenNoMatchExists()
		{
			var results = _service.LoadStats("NOPLAYER", "2025", "01").ToList();

			Assert.AreEqual(0, results.Count);
		}

		[TestMethod]
		public void LoadWeekShouldOnlyReturnRowsForRequestedSeasonAndWeek()
		{
			var results = _service.LoadWeek("2025", "01").ToList();

			Assert.AreEqual(3, results.Count);
			Assert.IsTrue(results.All(s => s.Season == "2025" && s.Week == "01"));
		}

		[TestMethod]
		public void HasWeekShouldOnlyReturnTrueForKnownWeek()
		{
			Assert.IsTrue(_service.HasWeek("2025", "01"));
			Assert.IsTrue(_service.HasWeek("2024", "01"));

			Assert.IsFalse(_service.HasWeek("2025", "03"));
			Assert.IsFalse(_service.HasWeek("2024", "02"));
		}
	}
}
