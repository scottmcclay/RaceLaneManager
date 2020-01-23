using System.Collections.Generic;
using Rlm.Core;

namespace Rlm.Web.DTOs
{
    public class TournamentDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int NumLanes { get; set; }
        public int TrackLengthInches { get; set; }
        public string State { get; set; }
        public List<CarDto> Cars { get; set; } = new List<CarDto>();
        public List<RaceDto> Races { get; set; } = new List<RaceDto>();
        public int CurrentRace { get; set; }

        public static TournamentDto FromTournament(ITournament tournament)
        {
            if (tournament == null) return null;

            TournamentDto result = new TournamentDto()
            {
                ID = tournament.ID,
                Name = tournament.Name,
                NumLanes = tournament.NumLanes,
                TrackLengthInches = tournament.TrackLengthInches,
                State = tournament.State.ToString(),
                CurrentRace = tournament.CurrentRace
            };

            foreach (ICar car in tournament.Cars)
            {
                result.Cars.Add(CarDto.FromCar(car));
            }

            foreach (IRace race in tournament.Races)
            {
                result.Races.Add(RaceDto.FromRace(race));
            }

            return result;
        }
    }
}
