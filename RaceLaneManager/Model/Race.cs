using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace RaceLaneManager.Model
{
    public enum RaceState
    {
        NotStarted,
        Racing,
        Done
    }

    public interface IRace
    {
        int RaceNumber { get; }
        RaceState State { get; }
        IEnumerable<ILaneAssignment> LaneAssignments { get; }
    }

    public class Race : IRace
    {
        [JsonIgnore]
        public List<LaneAssignment> LaneAssignmentData { get; set; }
        public int RaceNumber { get; set; }
        public RaceState State { get; set; }
        public IEnumerable<ILaneAssignment> LaneAssignments { get { return this.LaneAssignmentData; } }

        public Race()
        {
            this.LaneAssignmentData = new List<LaneAssignment>();
        }
    }
}
