using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class RaceViewModel
    {
        public int RaceNumber { get; private set; }
        public string RaceName => $"Race {this.RaceNumber}";

        public LaneAssignmentViewModel[] LaneAssignments { get; private set; }

        public RaceViewModel(IRace race)
        {
            this.RaceNumber = race.RaceNumber;

            List<LaneAssignmentViewModel> assignments = new List<LaneAssignmentViewModel>();
            foreach (var assignment in race.LaneAssignments)
            {
                assignments.Add(new LaneAssignmentViewModel(assignment));
            }
            this.LaneAssignments = assignments.ToArray();
        }
    }
}
