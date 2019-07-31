using System;

namespace Rlm.Core
{
    public delegate void TournamentUpdatedEventHandler(TournamentUpdatedEventArgs e);

    public class TournamentUpdatedEventArgs : EventArgs
    {
        public ITournament Tournament { get; set; }
        public TournamentUpdatedEventArgs(ITournament tournament)
        {
            this.Tournament = tournament;
        }
    }
}
