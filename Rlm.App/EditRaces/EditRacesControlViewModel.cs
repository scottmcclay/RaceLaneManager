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
    class EditRacesControlViewModel : IDisposable
    {
        public int TournamentID { get; private set; }

        public ObservableCollection<EditRaceViewModel> Races { get; private set; }
        public int CurrentRaceNumber { get; private set; }

        public EditRacesControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;

            TournamentManager.RacesUpdated += TournamentManager_RacesUpdated;
            TournamentManager.RaceUpdated += TournamentManager_RaceUpdated;
            TournamentManager.CurrentRaceChanged += TournamentManager_CurrentRaceChanged;

            this.Races = new ObservableCollection<EditRaceViewModel>();
            this.GetRaces();
        }

        public void GetRaces()
        {
            IRace currentRace = TournamentManager.GetCurrentRace(this.TournamentID);
            UpdateRaces(TournamentManager.GetRaces(this.TournamentID), currentRace);
        }

        private void UpdateRaces(RlmGetRacesResponse racesResponse, IRace currentRace = null)
        {
            this.Races.Clear();

            foreach (IRace race in racesResponse.Races)
            {
                EditRaceViewModel vm = new EditRaceViewModel(this.TournamentID, race);
                if ((currentRace != null) && (vm.RaceNumber == currentRace.RaceNumber))
                {
                    vm.CurrentRace = true;
                }

                this.Races.Add(vm);
            }
        }

        public void SaveRace(IRace race)
        {
            TournamentManager.UpdateRace(this.TournamentID, race);
        }

        public void SetCurrentRace(IRace race)
        {
            TournamentManager.SetCurrentRace(this.TournamentID, race.RaceNumber);
        }

        public void StartRace(IRace race)
        {
            TournamentManager.StartRace(this.TournamentID, race.RaceNumber);
        }

        public void StopRace(IRace race)
        {
            TournamentManager.StopRace(this.TournamentID, race.RaceNumber);
        }

        private void TournamentManager_RacesUpdated(int tournamentID, RacesUpdatedEventArgs e)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => this.TournamentManager_RacesUpdated(tournamentID, e));
            }
            else
            {
                if (tournamentID != this.TournamentID) return;

                UpdateRaces(e.Races);
            }
        }

        private void TournamentManager_RaceUpdated(int tournamentID, RaceUpdatedEventArgs e)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => this.TournamentManager_RaceUpdated(tournamentID, e));
            }
            else
            {
                if (tournamentID != this.TournamentID) return;

                foreach (EditRaceViewModel race in this.Races)
                {
                    if (race.RaceNumber == e.Race.RaceNumber)
                    {
                        race.Update(e.Race);
                        break;
                    }
                }
            }
        }

        private void TournamentManager_CurrentRaceChanged(int tournamentID, CurrentRaceChangedEventArgs e)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => this.TournamentManager_CurrentRaceChanged(tournamentID, e));
            }
            else
            {
                if (tournamentID != this.TournamentID) return;

                foreach (EditRaceViewModel race in this.Races)
                {
                    if (race.RaceNumber == e.CurrentRace.RaceNumber)
                    {
                        race.CurrentRace = true;
                    }
                    else
                    {
                        race.CurrentRace = false;
                    }
                }
            }
        }

        public void Dispose()
        {
            TournamentManager.RacesUpdated -= TournamentManager_RacesUpdated;
        }
    }
}
