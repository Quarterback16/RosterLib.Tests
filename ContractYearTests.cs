using RosterLib.Helpers;
using RosterLib.Implementations;

namespace RosterLib.Tests
{
    [TestClass]
    public class ContractYearTests
    {
        private ContractYearService? _sut;

        [TestInitialize]
        public void Setup()
        {
            //  make sure you re in Debug mode
            _sut = new ContractYearService();
        }

        [TestMethod]
        public void TestContractYears()
        {
            var result = _sut?.IsContractYear(
                name: "Kirk Cousins",
                season: "2026");
            Assert.IsTrue(result);
        }
    }
}
