using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rlm.App
{
    class EditRaceViewModel : IDisposable
    {
        private int _tournamentID;
        private IRace _race;
        private static readonly string RaceStateRacingDisplayText = "Racing";
        private static readonly string RaceStateDoneDisplayText = "Done";
        private static readonly string RaceStateNotStartedDisplayText = "Not Started";

        public int? RaceNumber => _race?.RaceNumber;
        public string RaceName => $"Race {_race?.RaceNumber}";
        public Visibility CurrentRaceVisibility { get; private set; }

        public string RaceState
        {
            get
            {
                string result = "Unknown";

                if (_race != null)
                {
                    switch (_race.State)
                    {
                        case Core.RaceState.Racing:
                            result = RaceStateRacingDisplayText;
                            break;

                        case Core.RaceState.Done:
                            result = RaceStateDoneDisplayText;
                            break;

                        case Core.RaceState.NotStarted:
                            result = RaceStateNotStartedDisplayText;
                            break;
                    }
                }

                return result;
            }
        }

        public IEnumerable<string> PossibleRaceStates => new string[]
        {
            RaceStateNotStartedDisplayText,
            RaceStateRacingDisplayText,
            RaceStateDoneDisplayText
        };

        public EditRaceViewModel(int tournamentID, IRace race)
        {
            _tournamentID = tournamentID;
            _race = race;
            UpdateCurrentRaceVisibility(TournamentManager.GetCurrentRace(_tournamentID)?.RaceNumber);
        }

        public void Dispose()
        {
            TournamentManager.CurrentRaceUpdated -= TournamentManager_CurrentRaceUpdated;
        }

        private void UpdateCurrentRaceVisibility(int? currentRaceNumber)
        {
            if (currentRaceNumber.HasValue && (currentRaceNumber.Value == _race.RaceNumber))
            {
                this.CurrentRaceVisibility = Visibility.Visible;
            }
            else
            {
                this.CurrentRaceVisibility = Visibility.Hidden;
            }
        }

        private void TournamentManager_CurrentRaceUpdated(int tournamentID, CurrentRaceUpdatedEventArgs e)
        {
            if (_tournamentID == tournamentID)
            {
                UpdateCurrentRaceVisibility(e.CurrentRace?.RaceNumber);
            }
        }
    }
}
