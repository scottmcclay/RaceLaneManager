using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.Core
{
    public delegate void LaneResultEventHandler(LaneResultEventArgs e);

    public class LaneResultEventArgs : EventArgs
    {
        public int TournamentID { get; set; }
        public int RaceNum { get; set; }
        public int LaneNum { get; set; }
        public long Time { get; set; }

        public LaneResultEventArgs(int tournamentID, int raceNum, int laneNum, long time)
        {
            this.TournamentID = tournamentID;
            this.RaceNum = raceNum;
            this.LaneNum = laneNum;
            this.Time = time;
        }
    }
}
