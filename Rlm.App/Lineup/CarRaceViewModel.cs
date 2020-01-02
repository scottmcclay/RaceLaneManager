using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class CarRaceViewModel
    {
        public int RaceNum { get; set; }
        public string RaceName { get; set; }
        public int LaneNum { get; set; }
        public string LaneName { get; set; }

        public CarRaceViewModel(RlmGetRacesResponse.CarRaces.CarLaneAssignment assignment)
        {
            this.RaceNum = assignment.RaceNum;
            this.RaceName = $"Race {assignment.RaceNum}";
            this.LaneNum = assignment.LaneNum;
            this.LaneName = $"Lane {assignment.LaneNum}";
        }
    }
}
