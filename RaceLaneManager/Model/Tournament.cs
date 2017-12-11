using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RaceLaneManager.Model
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

    public interface ITournament
    {
        int ID { get; }
        string Name { get; }
        int NumLanes { get; }
        TournamentState State { get; }
        IEnumerable<ICar> Cars { get; }
        IEnumerable<IRace> Races { get; }
    }

    public class Tournament : ITournament
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int NumLanes { get; set; }
        public TournamentState State { get; set; }

        [JsonIgnore]
        public List<Car> CarData { get; set; }
        public IEnumerable<ICar> Cars { get { return this.CarData; } }

        [JsonIgnore]
        public List<Race> RaceData { get; set; }
        public IEnumerable<IRace> Races { get { return this.RaceData; } }

        public Tournament()
        {
            this.CarData = new List<Car>();
            this.RaceData = new List<Race>();
            this.State = TournamentState.PreEvent;
        }
    }
}
