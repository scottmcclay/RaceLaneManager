using System;

namespace Rlm.Core
{
    public delegate void CurrentRaceUpdatedEventHandler(int tournamentID, CurrentRaceUpdatedEventArgs e);

    public class CurrentRaceUpdatedEventArgs : EventArgs
    {
        public IRace CurrentRace { get; set; }
        public CurrentRaceUpdatedEventArgs(IRace currentRace)
        {
            this.CurrentRace = currentRace;
        }
    }
}
