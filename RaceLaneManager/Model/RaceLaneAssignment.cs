using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceLaneManager.Model
{
    public class RaceLaneAssignment
    {
        public int RaceNum { get; set; }
        public Lane Lane { get; set; }
        public Racer Racer { get; set; }
        public long ElapsedTime { get; set; }
    }
}
