using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace Rlm.Core
{
    public enum TournamentState
    {
        PreEvent,
        EventReady,
        ReadyToRace,
        RaceInProgress,
        RaceFinalized,
        PostEvent
    }

    public interface ITournament : INotifyPropertyChanged
    {
        int ID { get; }
        string Name { get; }
        int NumLanes { get; }
        int TrackLengthInches { get; }
        int NumCars { get; }
        int NumRaces { get; }
        TournamentState State { get; }
        IEnumerable<ICar> Cars { get; }
        IEnumerable<IRace> Races { get; }
        int CurrentRace { get; }
    }

    public class Tournament : PropertyChangeNotifier, ITournament
    {
        [JsonIgnore]
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(this.ID));
                }
            }
        }

        [JsonIgnore]
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(this.Name));
                }
            }
        }

        [JsonIgnore]
        private int _numLanes;
        public int NumLanes
        {
            get { return _numLanes; }
            set
            {
                if (_numLanes != value)
                {
                    _numLanes = value;
                    OnPropertyChanged(nameof(this.NumLanes));
                }
            }
        }

        [JsonIgnore]
        private int _trackLengthInches = 480;
        public int TrackLengthInches
        {
            get => _trackLengthInches;
            set
            {
                if (_trackLengthInches != value)
                {
                    _trackLengthInches = value;
                    OnPropertyChanged(nameof(this.NumLanes));
                }
            }
        }

        public int NumCars{ get { return this.CarData.Count; } }

        public int NumRaces { get { return this.RaceData.Count; } }

        private TournamentState _state;
        public TournamentState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged(nameof(this.State));
                }
            }
        }

        [JsonIgnore]
        public List<Car> CarData { get; set; }
        public IEnumerable<ICar> Cars { get { return this.CarData; } }

        [JsonIgnore]
        public List<Race> RaceData { get; set; }
        public IEnumerable<IRace> Races { get { return this.RaceData; } }
        public int CurrentRace { get; set; }

        public Tournament()
        {
            this.State = TournamentState.PreEvent;
            this.CarData = new List<Car>();
            this.RaceData = new List<Race>();
            this.CurrentRace = 0;
        }
    }
}
