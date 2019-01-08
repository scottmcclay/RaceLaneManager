using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rlm.App
{
    class EditRaceViewModel : IRace, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _tournamentID;

        private static readonly Dictionary<RaceState, string> _raceStateStringMap = new Dictionary<RaceState, string>
        {
            { RaceState.NotStarted, "Not Started" },
            { RaceState.Racing, "Racing" },
            { RaceState.Done, "Done" }
        };

        public int RaceNumber { get; set; }
        public string RaceName => $"Race {this.RaceNumber}";

        private bool _currentRace = false;


        public bool CurrentRace
        {
            get => _currentRace;
            set
            {
                _currentRace = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentRace)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CurrentRaceVisibility)));
            }
        }
        public Visibility CurrentRaceVisibility => this.CurrentRace ? Visibility.Visible : Visibility.Hidden;

        public ObservableCollection<LaneAssignmentViewModel> LaneAssignmentViewModels { get; private set; }
        public IEnumerable<ILaneAssignment> LaneAssignments => this.LaneAssignmentViewModels;

        private RaceState _raceState;
        public RaceState State
        {
            get => _raceState;
            set
            {
                if (_raceState != value)
                {
                    _raceState = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.State)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RaceStateText)));
                }
            }
        }
        public string RaceStateText
        {
            get => _raceStateStringMap[this.State];
            set
            {
                foreach (var pair in _raceStateStringMap)
                {
                    if (value == pair.Value)
                    {
                        this.State = pair.Key;
                        return;
                    }
                }

                throw new ArgumentException($"{value} is not a valid RaceState");
            }
        }

        public IEnumerable<string> PossibleRaceStates => _raceStateStringMap.Values;

        public EditRaceViewModel(int tournamentID, IRace race)
        {
            _tournamentID = tournamentID;
            this.RaceNumber = race.RaceNumber;
            this.State = race.State;

            this.LaneAssignmentViewModels = new ObservableCollection<LaneAssignmentViewModel>();
            foreach (ILaneAssignment laneAssignment in race.LaneAssignments)
            {
                this.LaneAssignmentViewModels.Add(new LaneAssignmentViewModel(laneAssignment));
            }
        }

        public void Update(IRace race)
        {
            this.State = race.State;
            this.LaneAssignmentViewModels.Clear();
            foreach (ILaneAssignment laneAssignment in race.LaneAssignments)
            {
                this.LaneAssignmentViewModels.Add(new LaneAssignmentViewModel(laneAssignment));
            }
        }
    }
}
