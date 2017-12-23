using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceLaneManager.Model
{
    public interface ILaneAssignment
    {
        int Lane { get; }
        ICar Car { get; }
        long ElapsedTime { get; }
    }

    public class LaneAssignment : ILaneAssignment
    {
        public int Lane { get; set; }
        public ICar Car { get; set; }
        public long ElapsedTime { get; set; }
    }
}
