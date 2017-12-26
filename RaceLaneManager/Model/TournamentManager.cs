using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceLaneManager.Repository;
using RaceLaneManager.Engines;
using RaceLaneManager.DTOs;

namespace RaceLaneManager.Model
{
    public class TournamentManager
    {
        private static Object _lock = new Object();

        public static IEnumerable<ITournament> GetTournaments()
        {
            List<Tournament> result = new List<Tournament>();
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            lock (_lock)
            {
                foreach (int i in repo.GetAllTournamentIDs())
                {
                    result.Add(repo.LoadTournament(i));
                }
            }

            return result;
        }

        public static ITournament GetTournament(int tournamentID)
        {
            Tournament tournament = null;
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            lock (_lock)
            {
                tournament = repo.LoadTournament(tournamentID);
            }

            return tournament;
        }

        public static ITournament AddTournament(string name, int numLanes)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            Tournament tournament = new Tournament();

            lock (_lock)
            {
                tournament.Name = name;
                tournament.NumLanes = numLanes;

                // find an ID to assign to this new Tournament
                int maxID = 0;
                foreach (int i in repo.GetAllTournamentIDs())
                {
                    if (i > maxID)
                    {
                        maxID = i;
                    }
                }

                tournament.ID = maxID + 1;

                repo.SaveTournament(tournament);
            }

            return tournament;
        }

        public static ITournament UpdateTournament(int tournamentID, string newName, int numLanes)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            Tournament tournament = null;

            lock (_lock)
            {
                tournament = repo.LoadTournament(tournamentID);
                if (tournament == null)
                {
                    throw new ArgumentException(string.Format("Tournament with ID {0} does not exist.", tournamentID));
                }

                tournament.Name = newName;
                tournament.NumLanes = numLanes;

                repo.SaveTournament(tournament);
            }

            return tournament;
        }

        public static IEnumerable<ICar> GetCars(int tournamentID)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            Tournament tournament = null;

            lock (_lock)
            {
                tournament = repo.LoadTournament(tournamentID);
            }

            return tournament.CarData;
        }

        public static ICar AddCar(int tournamentID, ICar car)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();
            Car newCar = Car.From(car);

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);

                // find an ID to assign to this new Car
                int maxID = 0;
                foreach (Car c in tournament.CarData)
                {
                    if (c.ID > maxID)
                    {
                        maxID = c.ID;
                    }
                }

                newCar.ID = maxID + 1;
                tournament.CarData.Add(newCar);

                repo.SaveTournament(tournament);
            }

            return newCar;
        }

        public static ICar UpdateCar(int tournamentID, ICar car)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();
            Car updatedCar = null;

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);

                updatedCar = tournament.CarData.Where(c => c.ID == car.ID).SingleOrDefault();
                updatedCar.CopyFrom(car);

                repo.SaveTournament(tournament);
            }

            return updatedCar;
        }

        public static ICar DeleteCar(int tournamentID, int carID)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();
            Car deletedCar = null;

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);

                deletedCar = tournament.CarData.Where(c => c.ID == carID).SingleOrDefault();
                tournament.CarData.Remove(deletedCar);

                repo.SaveTournament(tournament);
            }

            return deletedCar;
        }

        private static void GenerateRaces(ref Tournament tournament)
        {
            tournament.RaceData = new List<Race>();

            RaceGenerator generator = new RaceGenerator();

            // rawRaces has a format of [race,lane] where race and lane are 0 based
            // the number at [race,lane] is the 1-based racer number
            int[,] rawRaces = generator.Solve(tournament.Cars.Count(), tournament.NumLanes);

            List<ICar> cars = new List<ICar>(tournament.Cars);

            for (int raceNum = 0; raceNum < rawRaces.GetLength(0); raceNum++)
            {
                Race race = new Race();
                race.RaceNumber = raceNum + 1;
                race.State = RaceState.NotStarted;
                race.LaneAssignmentData = new List<LaneAssignment>();

                for (int laneNum = 0; laneNum < rawRaces.GetLength(1); laneNum++)
                {
                    LaneAssignment laneAssignment = new LaneAssignment();
                    laneAssignment.Lane = laneNum + 1;
                    laneAssignment.ElapsedTime = 0;
                    laneAssignment.Car = Car.From(cars[rawRaces[raceNum, laneNum] - 1]);
                    race.LaneAssignmentData.Add(laneAssignment);
                }

                tournament.RaceData.Add(race);
            }

            if (rawRaces.GetLength(0) > 0)
            {
                tournament.CurrentRace = 0;
            }
        }

        public static RlmGetRacesResponse GenerateRaces(int tournamentID)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();
            RlmGetRacesResponse result = null;

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);

                // generate the races
                TournamentManager.GenerateRaces(ref tournament);

                repo.SaveTournament(tournament);
                result = GetRaces(tournament);
            }

            return result;
        }

        public static RlmGetRacesResponse GetRaces(Tournament tournament)
        {
            RlmGetRacesResponse response = new RlmGetRacesResponse();

            response.NumCars = tournament.NumCars;
            response.NumLanes = tournament.NumLanes;
            response.NumRaces = tournament.NumRaces;
            response.Races = tournament.Races;

            Dictionary<int, List<RlmGetRacesResponse.CarRaces.CarLaneAssignment>> carRaceDictionary = new Dictionary<int, List<RlmGetRacesResponse.CarRaces.CarLaneAssignment>>();
            foreach (IRace race in tournament.Races)
            {
                foreach (ILaneAssignment assignment in race.LaneAssignments)
                {
                    if (!carRaceDictionary.ContainsKey(assignment.Car.ID))
                    {
                        carRaceDictionary.Add(assignment.Car.ID, new List<RlmGetRacesResponse.CarRaces.CarLaneAssignment>());
                    }

                    carRaceDictionary[assignment.Car.ID].Add(new RlmGetRacesResponse.CarRaces.CarLaneAssignment() { RaceNum = race.RaceNumber, LaneNum = assignment.Lane });
                }
            }

            List<RlmGetRacesResponse.CarRaces> racesByCar = new List<RlmGetRacesResponse.CarRaces>();
            foreach (int carID in carRaceDictionary.Keys)
            {
                RlmGetRacesResponse.CarRaces carRaces = new RlmGetRacesResponse.CarRaces();
                carRaces.Car = tournament.Cars.Where(c => c.ID == carID).SingleOrDefault();
                carRaces.Assignments = carRaceDictionary[carID].OrderBy(c => c.RaceNum).ToList();
                racesByCar.Add(carRaces);
            }

            response.RacesByCar = racesByCar;

            return response;
        }

        public static RlmGetRacesResponse GetRaces(int tournamentID)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();
            RlmGetRacesResponse response = null;

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);
                response = GetRaces(tournament);
            }

            return response;
        }

        public static IRace GetCurrentRace(int tournamentID)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();
            IRace result = null;

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);
                foreach (Race race in tournament.Races.OrderBy(r => r.RaceNumber))
                {
                    if (race.State != RaceState.Done)
                    {
                        result = race;
                        break;
                    }
                }
            }

            return result;
        }

        public static string GetPositionText(int position)
        {
            if (position <= 0)
            {
                return position.ToString();
            }

            switch (position % 100)
            {
                case 11:
                case 12:
                case 13:
                    return string.Format("{0}th", position);
            }

            switch (position % 10)
            {
                case 1:
                    return string.Format("{0}st", position);

                case 2:
                    return string.Format("{0}nd", position);

                case 3:
                    return string.Format("{0}rd", position);
            }

            return string.Format("{0}th", position);
        }

        public static IEnumerable<IStanding> GetStandings(Tournament tournament)
        {
            IEnumerable<IStanding> result = new List<Standing>();

            Dictionary<int, Standing> carPoints = new Dictionary<int, Standing>();
            foreach (Car car in tournament.CarData)
            {
                Standing standing = new Standing();
                standing.Car = car;
                standing.Points = 0;
                carPoints.Add(car.ID, standing);
            }

            foreach (Race race in tournament.RaceData)
            {
                foreach (LaneAssignment assignment in race.LaneAssignmentData)
                {
                    carPoints[assignment.Car.ID].Points += assignment.Points;
                }
            }

            List<KeyValuePair<int, Standing>> orderedCarPoints = carPoints.OrderByDescending(cp => cp.Value.Points).ToList();
            int index = 0;
            int position = 1;
            while (index < orderedCarPoints.Count())
            {
                orderedCarPoints[index].Value.Position = GetPositionText(position);

                if (((index + 1) >= orderedCarPoints.Count()) || 
                    (orderedCarPoints[index + 1].Value.Points != orderedCarPoints[index].Value.Points))
                {
                    position++;
                }
                index++;
            }

            result = orderedCarPoints.Select(o => o.Value);

            return result;
        }

        public static IEnumerable<IStanding> GetStandings(int tournamentID)
        {
            IEnumerable<IStanding> result = null;
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);
                result = GetStandings(tournament);
            }

            return result;
        }

        public static IEnumerable<IRace> GetNextRaces(int tournamentID)
        {
            List<Race> result = new List<Race>();
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);

                if (tournament.CurrentRace >= 0)
                {
                    if (tournament.CurrentRace + 1 < tournament.RaceData.Count)
                    {
                        result.Add(tournament.RaceData[tournament.CurrentRace + 1]);
                    }

                    if (tournament.CurrentRace + 2 < tournament.RaceData.Count)
                    {
                        result.Add(tournament.RaceData[tournament.CurrentRace + 2]);
                    }
                }
            }

            return result;
        }
    }
}
