using RosterLib.Helpers;
using RosterLib.Implementations;
using RosterLib.Services;
using TFLLib;

namespace RosterLib.Tests
{
    [TestClass]
    public class PlayerCsvTests
    {
        [TestMethod]
        public void PlayerCsv_ProducesWideCsv()
        {
            var timekeeper = new TimeKeeper(clock:null);
            var sut = new PlayerCsv(
                timekeeper,
                new AdpMaster(
                    $"{ConfigHelper.CsvFolder()}ADP {timekeeper.CurrentSeason()}.csv"),
                new DoozyService(
                    timekeeper.CurrentSeason(),
                    ConfigHelper.JsonFolder()),
                new ContractYearService(
                    timekeeper.CurrentSeason(),
                    ConfigHelper.JsonFolder()),
                new ProjectionService(
                    new DbfPlayerGameMetricsDao(),
                        new DataLibrarian(
                            Utility.NflConnectionString(),
                            Utility.TflConnectionString(),
                            Utility.CtlConnectionString(),
                            null)))
                        {
                            DoProjections = true  // 2024-06-01 decided to stick to one CSV format as it feeds into a lot of stuff
                        };
            sut.RenderPlayerCsv();
        }
    }
}
