using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Rlm.Core;

namespace Rlm.App
{
    class RaceDetailsControlViewModel : INotifyPropertyChanged
    {
        private IRace _race;

        public event PropertyChangedEventHandler PropertyChanged;

        public int TournamentID { get; private set; }

        public string CurrentRace
        {
            get
            {
                string result = "Race ??";
                if (_race != null)
                {
                    result = $"Race {_race.RaceNumber}";
                }

                return result;
            }
        }

        public string RaceState
        {
            get
            {
                string result = "Unknown";
                if (_race != null)
                {
                    switch (_race.State)
                    {
                        case Core.RaceState.NotStarted:
                            result = "Lining Up";
                            break;

                        case Core.RaceState.Racing:
                            result = "Racing";
                            break;

                        case Core.RaceState.Done:
                            result = "Complete";
                            break;
                    }
                }

                return result;
            }
        }

        public ObservableCollection<LaneAssignmentViewModel> LaneAssignments { get; private set; }

        public RaceDetailsControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;
            TournamentManager.RaceUpdated += TournamentManager_RaceUpdated;
            TournamentManager.CurrentRaceChanged += TournamentManager_CurrentRaceChanged;
            _race = TournamentManager.GetCurrentRace(this.TournamentID);

            this.LaneAssignments = new ObservableCollection<LaneAssignmentViewModel>();
            UpdateLanes(_race);
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

                _race = e.CurrentRace;

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentRace)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RaceState)));
                UpdateLanes(_race);
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
                if ((_race == null) || (tournamentID != this.TournamentID) || (e.Race.RaceNumber != _race.RaceNumber)) return;

                _race = e.Race;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentRace)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RaceState)));
                UpdateLanes(_race);
            }
        }

        private void UpdateLanes(IRace race)
        {
            this.LaneAssignments.Clear();
            if (_race != null)
            {
                foreach (ILaneAssignment assignment in _race.LaneAssignments)
                {
                    this.LaneAssignments.Add(new LaneAssignmentViewModel(assignment));
                }
            }
        }
    }
}
