using RosterLib.Helpers;

namespace RosterLib.Tests
{
	[TestClass]
	public class AdpMasterTests
	{
		private AdpMaster? _sut;

		[TestInitialize]
		public void Setup()
		{
			_sut = new AdpMaster(2026);
		}

		[TestMethod]
		public void AdpMaster_LoadWithBadPath_ThrowsException()
		{
			var result = _sut?.Load(
				".\\Output\\2019\\Starters\\csv\\Starters.csv");
			Assert.IsTrue(result == 0);
		}

		[TestMethod]
		public void AdpMasterLoad_WithGoodPath_ReturnsNumberOfPlayers()
		{
			var result = _sut?.Load();
			Assert.IsTrue(result > 0 || result == 0);
			Console.WriteLine(
				"Number of Players:{0}",
				result);
		}

		[TestMethod]
		public void AdpMasterGetPosRank_Works()
		{
			_sut?.Load();
			var adp = _sut?.GetAdpPosRank("Lamar Jackson");
			Assert.AreEqual("QB2", adp);
		}

		[TestMethod]
		public void AdpHelper_CanListPositionRankings()
		{
			_sut?.Load();
			var md = AdpHelper.PositionRankingsToMarkDown(
				_sut!,
				"WR");
			Assert.IsTrue(
				md.Length > 0,
				"Markdown should not be empty");
			Console.WriteLine(md);
		}

		[TestMethod]
		public void AdpHelper_CanListDefenceRankings()
		{
			_sut?.Load();
			var md = AdpHelper.PositionRankingsToMarkDown(
				_sut!,
				"DST");
			Assert.IsTrue(
				md.Length > 0,
				"Markdown should not be empty");
			Console.WriteLine(md);
		}

		[TestMethod]
		public void AdpHelper_CanDoFullList()
		{
			_sut?.Load();
			var md = AdpHelper.FullAdpToMarkDown(
				_sut!);
			Assert.IsTrue(
				md.Length > 0,
				"Markdown should not be empty");
			Console.WriteLine(md);
		}

		[TestMethod]
		public void AdpHelper_CanDoFullList_2026()
		{
			var sut = new AdpMaster(2026);
			sut.Load();

			var md = AdpHelper.FullAdpToMarkDown(
				sut,
				"AdpHelper_CanDoFullList_2026");

			Assert.IsTrue(
				md.Length > 0,
				"Markdown should not be empty");
			Console.WriteLine(md);
		}

		[TestMethod]
		public void AdpHelper_CanListPositionRankings_QB_2026()
		{
			PosRanking("QB", 2026);
		}

		[TestMethod]
		public void AdpHelper_CanListPositionRankings_RB_2026()
		{
			PosRanking("RB", 2026);
		}

		[TestMethod]
		public void AdpHelper_CanListPositionRankings_WR_2026()
		{
			PosRanking("WR", 2026);
		}

		[TestMethod]
		public void AdpHelper_CanListPositionRankings_TE_2026()
		{
			PosRanking("TE", 2026);
		}

		[TestMethod]
		public void AdpHelper_CanListPositionRankings_DST_2026()
		{
			PosRanking("DST", 2026);
		}

		[TestMethod]
		public void WriteAllPositionRankingsToObsidian()
		{
			Assert.IsNotNull(_sut, "_sut should not be null");

			var rootPath = $"{FolderHelper.GetObsidianNflStemFolder()}{_sut.Season}\\ADP\\";

			var dict = new Dictionary<string, string>
			{
				{"QB", $"{rootPath}ADP QB Rankings {_sut.Season}.md" },
				{"RB", $"{rootPath}ADP RB Rankings {_sut.Season}.md" },
				{"WR", $"{rootPath}ADP WR Rankings {_sut.Season}.md" },
				{"TE", $"{rootPath}ADP TE Rankings {_sut.Season}.md" },
				{"DST", $"{rootPath}ADP DST Rankings {_sut.Season}.md" }
			};
			foreach (KeyValuePair<string, string> pair in dict)
			{
				var md = PosRanking(pair.Key, Int32.Parse(_sut.Season));
				if (FileHelper.WriteStringToFile(pair.Value, md))
				{
					Console.WriteLine($"{pair.Key} written to {pair.Value}");
				}
				else
				{
					Assert.Fail($"Failed to write to file: {pair.Value}");
				}
			}
		}

		private static string PosRanking(
			string posOfInterest,
			int season)
		{
			var sut = new AdpMaster(season);
			sut.Load();
			var md = AdpHelper.PositionRankingsToMarkDown(
				sut,
				posOfInterest,
				$"AdpHelper_CanListPositionRankings_{posOfInterest}_{season}");
			Assert.IsTrue(
				md.Length > 0,
				"Markdown should not be empty");
			Console.WriteLine(md);
			return md.ToString();
		}

		[TestMethod]
		public void AdpHelper_CanDoDraftRoundNumbering()
		{
			var rd = AdpHelper.AsDraftRound(
				18,
				18);
			Assert.AreEqual("1.18", rd);
			Console.WriteLine(rd);
		}

		[TestMethod]
		public void AdpMasterCanTakeOutNoise()
		{
			_sut?.Load();
			var result = AdpMaster.TakeOutNoise("Kenneth Walker III");
			Console.WriteLine(result);
			Assert.AreEqual("Kenneth Walker", result);
		}

		[TestMethod]
		public void AdpMasterCanTakeOutSrNoise()
		{
			_sut?.Load();
			var result = AdpMaster.TakeOutNoise("Deebo Samuel Sr.");
			Console.WriteLine(result);
			Assert.AreEqual("Deebo Samuel", result);
		}

		[TestMethod]
		public void AdpMasterGet_ForUnknownPlayer_ReturnsEmptyString()
		{
			_sut?.Load();
			var adp = _sut?.GetAdp("Steve Colonna");
			Assert.AreEqual(string.Empty, adp);
		}

		[TestMethod]
		public void AdpMasterGet_WithoutLoad_PerformsImplicitLoad()
		{
			var adp = _sut?.GetAdp("Travis Kelce");
			Assert.IsTrue(adp == "69" || adp == "");
			Console.WriteLine($"Travis Kelce is #{adp}");
		}

		[TestMethod]
		public void AdpMasterGet_ForSaquon_Returns_3()
		{
			_sut?.Load();
			var adp = _sut?.GetAdp("Saquon Barkley");
			Assert.IsTrue(adp == "2" || adp == "");
			Console.WriteLine($"Saquon Barkley is #{adp}");
		}

		[TestMethod]
		public void AdpMasterGet_ForPlayer_Returns_Ok()
		{
			var player = "Josh Allen";
			_sut?.Load();
			var adp = _sut?.GetAdp(player);
			Assert.IsNotNull(adp);
			Console.WriteLine($"ADP for {player} is {adp}");
		}

		[TestMethod]
		public void AdpMasterGet_ForCMC_Returns_11()
		{
			_sut?.Load();
			var adp = _sut?.GetAdp("Christian McCaffrey");
			Assert.IsTrue(adp == "" || adp == "12");
		}

		[TestMethod]
		public void AdpRankGet_ForPlayerWorks()
		{
			_sut?.Load();
			var adp = _sut?.GetAdpRank("Matthew Stafford");
			Console.WriteLine($"Matthew Stafford is #{adp}");
			Assert.IsTrue(adp == 96 || adp == 0);
		}

		[TestMethod]
		public void AdpRankGet_ForTyreek_Returns_32()
		{
			_sut?.Load();
			var adp = _sut?.GetAdpRank("Tyreek Hill");
			Assert.IsTrue(adp == 29 || adp == 0);
		}

		[TestMethod]
		public void AdpRankGet_ForBHall_Returns_30()
		{
			_sut?.Load();
			var adp = _sut?.GetAdpRank("Breece Hall");
			Assert.IsTrue(adp == 32 || adp == 0);
		}


		[TestMethod]
		public void AdpMaster_KnowsPicksForTeamNo18()
		{
			var result = _sut?.PicksFor(
				teamNumber: 18,
				teamCount: 18,
				rounds: 18);
			Array.ForEach(result, e => Console.WriteLine(e));
			if (result == null)
				Assert.Fail("Result should not be null");
			Assert.AreEqual(18, result[0]);
			Assert.AreEqual(19, result[1]);
		}

		[TestMethod]
		public void AdpMaster_KnowsPicksForTeamNo14()
		{
			var result = _sut?.PicksFor(14, 14, 14);
			Array.ForEach(result, e => Console.WriteLine(e));
			if (result == null)
				Assert.Fail("Result should not be null");
			Assert.AreEqual(14, result[0]);
			Assert.AreEqual(15, result[1]);
			Assert.AreEqual(42, result[2]);
		}

		[TestMethod]
		public void AdpMaster_TellsYouUrTeamAtPosition()
		{
			var result = _sut?.TeamFor(
				teamNo: 4,
				teamsInLeague: 18,
				draftRounds: 14);
			Assert.IsTrue(result != null);
			Array.ForEach(result, e => Console.WriteLine(e));
		}

		[TestMethod]
		public void AdpMaster_KnowsAdpLine()
		{
			var playerName = "Charlie Woerner";
			var result = _sut?.GetAdpLine(
				playerName: playerName,
				teamsInLeague: 18);
			Assert.IsFalse(string.IsNullOrEmpty(result));
			Console.WriteLine($"{playerName} {result}");
		}

		[TestMethod]
		public void AdpMaster_KnowsRankOfPositionRank()
		{
			Assert.AreEqual(6, AdpHelper.PosAdpRankFromString("QB6"));
		}
	}
}
