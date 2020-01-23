using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rlm.Core;

namespace Rlm.Web.DTOs
{
    public class RaceDto
    {
        public int RaceNumber { get; set; }
        public string State { get; set; }
        public List<LaneAssignmentDto> LaneAssignments { get; } = new List<LaneAssignmentDto>();

        public static RaceDto FromRace(IRace race)
        {
            if (race == null) return null;

            RaceDto result = new RaceDto()
            {
                RaceNumber = race.RaceNumber,
                State = race.State.ToString()
            };

            foreach (ILaneAssignment laneAssignment in race.LaneAssignments)
            {
                result.LaneAssignments.Add(LaneAssignmentDto.FromLaneAssignment(laneAssignment));
            }

            return result;
        }
    }
}
