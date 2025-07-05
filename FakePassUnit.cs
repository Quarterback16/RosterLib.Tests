using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib.Tests
{
    public class FakePassUnit : PassUnit
    {
        public FakePassUnit()
        {
            Loader = new FakeLoadPassUnit();
        }
    }
}
