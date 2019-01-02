using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class LaneAssignmentViewModel
    {
        public int LaneNumber { get; private set; }
        public CarViewModel Car { get; private set; }
        public int EllapsedMilliseconds { get; private set; }
        public double EllapsedSeconds => this.EllapsedMilliseconds / 1000;
        public int VirtualSpeed { get; private set; }
        public int Position { get; private set; }
        public int Points { get; private set; }
    }
}
