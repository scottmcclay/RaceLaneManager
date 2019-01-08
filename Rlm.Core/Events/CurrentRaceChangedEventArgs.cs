using System;

namespace Rlm.Core
{
    public delegate void CurrentRaceChangedEventHandler(int tournamentID, CurrentRaceChangedEventArgs e);

    public class CurrentRaceChangedEventArgs : EventArgs
    {
        public IRace PreviousRace { get; private set; }
        public IRace CurrentRace { get; private set; }

        public CurrentRaceChangedEventArgs(IRace previousRace, IRace currentRace)
        {
            this.CurrentRace = currentRace;
            this.PreviousRace = previousRace;
        }
    }
}
