using System;
using System.Collections.Generic;

namespace Rlm.Core
{
    public delegate void StandingsUpdatedEventHandler(int tournamentID, StandingsUpdatedEventArgs e);

    public class StandingsUpdatedEventArgs : EventArgs
    {
        public IEnumerable<IStanding> Standings { get; set; }
        public StandingsUpdatedEventArgs(IEnumerable<IStanding> standings)
        {
            this.Standings = standings;
        }
    }
}
