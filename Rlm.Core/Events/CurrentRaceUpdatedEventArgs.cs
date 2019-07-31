using System;

namespace Rlm.Core
{
    public delegate void RaceUpdatedEventHandler(int tournamentID, RaceUpdatedEventArgs e);

    public class RaceUpdatedEventArgs : EventArgs
    {
        public IRace Race { get; set; }
        public RaceUpdatedEventArgs(IRace currentRace)
        {
            this.Race = currentRace;
        }
    }
}
