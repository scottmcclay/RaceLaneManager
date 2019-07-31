using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class LaneAssignmentViewModel : ILaneAssignment
    {
        public int Lane { get; set; }
        public string LaneText => $"Lane {this.Lane}";
        public CarViewModel Car { get; private set; }
        ICar ILaneAssignment.Car => this.Car;
        public double ElapsedSeconds { get; set; }
        public double ScaleSpeed { get; private set; }
        public string ScaleSpeedText => $"{ScaleSpeed.ToString("N1")} mph";
        public int Position { get; set; }
        public int Points { get; set; }
        public long ElapsedTime => (long)(this.ElapsedSeconds * 100000);


        public LaneAssignmentViewModel(ILaneAssignment laneAssignment)
        {
            if (laneAssignment == null) throw new ArgumentNullException(nameof(laneAssignment));

            this.Car = new CarViewModel(laneAssignment.Car);
            UpdateValues(laneAssignment);
        }

        public void UpdateValues(ILaneAssignment laneAssignment)
        {
            this.Lane = laneAssignment.Lane;
            this.Car = new CarViewModel(laneAssignment.Car);
            this.ElapsedSeconds = ((double)laneAssignment.ElapsedTime) / 100000;
            this.ScaleSpeed = laneAssignment.ScaleSpeed;
            this.Points = laneAssignment.Points;
            this.Position = laneAssignment.Position;
        }
    }
}
