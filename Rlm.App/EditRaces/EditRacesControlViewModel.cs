using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class EditRacesControlViewModel : IDisposable
    {
        public int TournamentID { get; private set; }

        public ObservableCollection<EditRaceViewModel> Races { get; private set; }
        public int CurrentRaceNumber { get; private set; }

        public EditRacesControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;

            TournamentManager.RacesUpdated += TournamentManager_RacesUpdated;
            this.Races = new ObservableCollection<EditRaceViewModel>();
            this.GetRaces();
        }

        public void GetRaces()
        {
            UpdateRaces(TournamentManager.GetRaces(this.TournamentID));
        }

        private void UpdateRaces(RlmGetRacesResponse racesResponse)
        {
            this.Races.Clear();

            foreach (IRace race in racesResponse.Races)
            {
                this.Races.Add(new EditRaceViewModel(this.TournamentID, race));
            }
        }

        private void TournamentManager_RacesUpdated(int tournamentID, RacesUpdatedEventArgs e)
        {
            UpdateRaces(e.Races);
        }

        public void Dispose()
        {
            TournamentManager.RacesUpdated -= TournamentManager_RacesUpdated;
        }
    }
}
