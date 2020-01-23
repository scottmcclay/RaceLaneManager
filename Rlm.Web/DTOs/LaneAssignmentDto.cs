using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rlm.Core;

namespace Rlm.Web.DTOs
{
    public class LaneAssignmentDto
    {
        public int Lane { get; set; }
        public CarDto Car { get; set; }
        public long ElapsedTime { get; set; }
        public double ScaleSpeed { get; set; }
        public int Position { get; set; }
        public int Points { get; set; }

        public static LaneAssignmentDto FromLaneAssignment(ILaneAssignment laneAssignment)
        {
            if (laneAssignment == null) return null;

            LaneAssignmentDto result = new LaneAssignmentDto()
            {
                Lane = laneAssignment.Lane,
                Car = CarDto.FromCar(laneAssignment.Car),
                ElapsedTime = laneAssignment.ElapsedTime,
                ScaleSpeed = laneAssignment.ScaleSpeed,
                Position = laneAssignment.Position,
                Points = laneAssignment.Points
            };

            return result;
        }
    }
}
