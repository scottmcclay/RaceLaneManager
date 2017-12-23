﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace RaceLaneManager.Model
{
    public interface IRace
    {
        int RaceNumber { get; }
        IEnumerable<ILaneAssignment> LaneAssignments { get; }
    }

    public class Race : IRace
    {
        [JsonIgnore]
        public List<LaneAssignment> LaneAssignmentData { get; set; }
        public int RaceNumber { get; set; }
        public IEnumerable<ILaneAssignment> LaneAssignments { get { return this.LaneAssignmentData; } }

        public Race()
        {
            this.LaneAssignmentData = new List<LaneAssignment>();
        }
    }
}
