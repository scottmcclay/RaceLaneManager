using System;
using System.Collections.Generic;

namespace Rlm.Core
{
    public delegate void NextRacesUpdatedEventHandler(int tournamentID, NextRacesUpdatedEventArgs e);

    public class NextRacesUpdatedEventArgs : EventArgs
    {
        public IEnumerable<IRace> NextRaces { get; set; }
        public NextRacesUpdatedEventArgs(IEnumerable<IRace> nextRaces)
        {
            this.NextRaces = nextRaces;
        }
    }
}
