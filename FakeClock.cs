using RosterLib.Interfaces;

namespace RosterLib.Tests
{
    public class FakeClock : IClock
    {
        public FakeClock(DateTime theDateTime)
        {
            Now = theDateTime;
        }

        public DateTime Now { get; private set; }

        public int GetMonth() => Now.Month;
    }
}
