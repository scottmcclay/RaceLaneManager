using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class UpNextControlViewModel
    {
        public int TournamentID { get; private set; }

        public ObservableCollection<StandingViewModel> Standings { get; private set; }

        public UpNextControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;
            this.Standings = new ObservableCollection<StandingViewModel>();

            PopulateStandings();
        }

        private void PopulateStandings()
        {
            foreach (IStanding standing in TournamentManager.GetStandings(this.TournamentID))
            {
                Standings.Add(new StandingViewModel(standing));
            }
        }
    }
}
