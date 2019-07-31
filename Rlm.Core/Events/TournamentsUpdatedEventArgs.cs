using System;
using System.Collections.Generic;

namespace Rlm.Core
{
    public delegate void TournamentsUpdatedEventHandler(TournamentsUpdatedEventArgs e);

    public class TournamentsUpdatedEventArgs : EventArgs
    {
        public IEnumerable<ITournament> Tournaments { get; set; }
        public TournamentsUpdatedEventArgs(IEnumerable<ITournament> tournaments)
        {
            this.Tournaments = tournaments;
        }
    }
}
