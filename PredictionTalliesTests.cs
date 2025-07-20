using Dumpify;
using System.Data;
using TFLLib;

namespace RosterLib.Tests
{
	[TestClass]
	public class PredictionTalliesTests
	{
		[TestMethod]
		public void TestPredictionTallies()
		{
			var season = "2025";

			var _dataLibrarian = new DataLibrarian(
					Utility.NflConnectionString(),
					Utility.TflConnectionString(),
					Utility.CtlConnectionString(),
					null);

			var predictions = _dataLibrarian.GetAllPredictions(
				season,
				method: "unit");

			var resultList = new List<GamePrediction>();
			foreach (DataRow dr in predictions.Tables[0].Rows)
				resultList.Add(
					new GamePrediction
					{
						YDr = Int32.Parse(dr["hydr"].ToString())
							+ Int32.Parse(dr["aydr"].ToString()),
						YDp = Int32.Parse(dr["hydp"].ToString())
							+ Int32.Parse(dr["aydp"].ToString()),
						TDr = Int32.Parse(dr["htdr"].ToString())
							+ Int32.Parse(dr["atdr"].ToString()),
						TDp = Int32.Parse(dr["htdp"].ToString())
							+ Int32.Parse(dr["atdp"].ToString()),
						TDs = Int32.Parse(dr["htds"].ToString())
							+ Int32.Parse(dr["atds"].ToString()),
						TDd = Int32.Parse(dr["htdd"].ToString())
							+ Int32.Parse(dr["atdd"].ToString()),
						FGs = Int32.Parse(dr["hfg"].ToString())
							+ Int32.Parse(dr["afg"].ToString()),
						Week = Int32.Parse(dr["week"].ToString()),
					});
			var totals = new List<GamePrediction>();
			for (int w = 1; w < 19; w++)
			{
				var week = new GamePrediction();
				week.Week = w;
				var week1 = resultList
					.Where(rl => rl.Week == w);
				foreach (var p in week1)
				{
					week.TDp += p.TDp;
					week.TDr += p.TDr;
					week.TDs += p.TDs;
					week.TDd += p.TDd;
					week.FGs += p.FGs;
					week.YDr += p.YDr;
					week.YDp += p.YDp;
				}
				totals.Add(week);
			}
			Assert.IsNotNull(totals);
			totals.Dump();
			Totals(resultList);
		}

		private void Totals(
			List<GamePrediction> resultList)
		{
			resultList
				.Sum(r => r.TDr)
				.Dump("TDR");
			resultList
				.Sum(r => r.TDp)
				.Dump("TDP");
			resultList
				.Sum(r => r.TDs)
				.Dump("TDS");
			resultList
				.Sum(r => r.YDr)
				.Dump("YDr");
			resultList
				.Sum(r => r.YDp)
				.Dump("YDp");
		}
	}

	public class GamePrediction
	{
		public int Week { get; set; }
		public int TDr { get; set; }
		public int TDp { get; set; }
		public int TDs { get; set; }
		public int TDd { get; set; }
		public int FGs { get; set; }
		public int YDr { get; set; }
		public int YDp { get; set; }
	}
}
