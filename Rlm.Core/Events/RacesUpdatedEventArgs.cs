using System;
using System.Collections.Generic;

namespace Rlm.Core
{
    public delegate void RacesUpdatedEventHandler(int tournamentID, RacesUpdatedEventArgs e);

    public class RacesUpdatedEventArgs : EventArgs
    {
        public RlmGetRacesResponse Races { get; set; }
        public RacesUpdatedEventArgs(RlmGetRacesResponse races)
        {
            this.Races = races;
        }
    }
}
