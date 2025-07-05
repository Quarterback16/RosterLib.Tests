namespace RosterLib.Tests
{
    public class FakeRushUnit : RushUnit
    {
        public FakeRushUnit()
        {
            Loader = new FakeLoadRunners();
        }
    }
}
