using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace RosterLib.Tests
{
    [TestClass]
    public class RunReportTests
    {
        [TestMethod]
        public void RunReport_RendersHtml()
        {
            var rr = new RunReport(timekeeper: new TimeKeeper(clock:null))
            {
                NoOfDaysBack = 7
            };
            rr.RenderAsHtml();
            var fileOut = rr.OutputFilename();
            Assert.IsTrue(
                File.Exists(fileOut),
                $"Cannot find {fileOut}");
        }
    }
}
