using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.Core
{
    public class Lane
    {
        public int LaneNum { get; set; }
        public bool Active { get; set; }

        public Lane(int laneNum, bool active = true)
        {
            this.LaneNum = laneNum;
            this.Active = active;
        }
    }
}
