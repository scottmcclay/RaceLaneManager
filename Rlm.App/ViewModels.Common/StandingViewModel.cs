using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class StandingViewModel
    {
        public string Position { get; private set; }
        public CarViewModel Car { get; private set; }
        public string Points { get; private set; }
        public string AverageTimeShort { get; private set; }
        public string AverageTime { get; private set; }
        public string AverageSpeed { get; private set; }

        public StandingViewModel(IStanding standing)
        {
            this.Position = standing.Position;
            this.Car = new CarViewModel(standing.Car);
            this.Points = $"{standing.Points} pts";
            double averageTime = ((double)standing.AverageTime) / 100000;
            this.AverageTimeShort = averageTime.ToString("N3");
            this.AverageTime = $"Avg {averageTime.ToString("N3")} sec";
            this.AverageSpeed = $"Avg {standing.AverageSpeed.ToString("N1")} mph";
        }
    }
}
