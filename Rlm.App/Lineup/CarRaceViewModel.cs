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
        public string RaceName { get; set; }
        public string LaneName { get; set; }

        public CarRaceViewModel(RlmGetRacesResponse.CarRaces.CarLaneAssignment assignment)
        {
            this.RaceName = $"Race {assignment.RaceNum}";
            this.LaneName = $"Lane {assignment.LaneNum}";
        }
    }
}
