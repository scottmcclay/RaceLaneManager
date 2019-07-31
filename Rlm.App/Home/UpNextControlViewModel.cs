using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rlm.App
{
    class UpNextControlViewModel
    {
        public int TournamentID { get; private set; }

        public ObservableCollection<UpNextRaceViewModel> NextRaces { get; private set; }

        public UpNextControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;
            this.NextRaces = new ObservableCollection<UpNextRaceViewModel>();

            TournamentManager.NextRacesUpdated += TournamentManager_NextRacesUpdated;

            PopulateNextRaces(TournamentManager.GetNextRaces(this.TournamentID));
        }

        private void TournamentManager_NextRacesUpdated(int tournamentID, NextRacesUpdatedEventArgs e)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => this.TournamentManager_NextRacesUpdated(tournamentID, e));
            }
            else
            {
                if (tournamentID != this.TournamentID) return;

                PopulateNextRaces(e.NextRaces);
            }
        }

        private void PopulateNextRaces(IEnumerable<IRace> races)
        {
            this.NextRaces.Clear();

            foreach (IRace race in races)
            {
                this.NextRaces.Add(new UpNextRaceViewModel(race));
            }
        }
    }
}
