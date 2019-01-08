using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class UpNextRaceViewModel
    {
        public string RaceName { get; set; }
        public List<CarViewModel> Cars { get; private set; }

        public UpNextRaceViewModel(IRace race)
        {
            this.RaceName = $"Race {race.RaceNumber}";
            this.Cars = new List<CarViewModel>();

            foreach (ILaneAssignment lane in race.LaneAssignments)
            {
                this.Cars.Add(new CarViewModel(lane.Car));
            }
        }
    }
}
