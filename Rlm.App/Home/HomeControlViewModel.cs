using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class HomeControlViewModel
    {
        public int TournamentID { get; private set; }

        public StandingsControlViewModel Standings { get; private set; }
        public UpNextControlViewModel UpNext { get; private set; }

        public HomeControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;

            this.Standings = new StandingsControlViewModel(this.TournamentID);
            this.UpNext = new UpNextControlViewModel(this.TournamentID);
        }
    }
}
