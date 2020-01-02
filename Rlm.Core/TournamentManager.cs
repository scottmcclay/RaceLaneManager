using System;
using System.Collections.Generic;
using System.Linq;
using ChaoticRotation.Core;

namespace Rlm.Core
{
    public class TournamentManager
    {
        private static Object _lock = new Object();

        public static event CarsUpdatedEventHandler CarsUpdated;
        public static event CarUpdatedEventHandler CarUpdated;
        public static event RaceUpdatedEventHandler RaceUpdated;
        public static event CurrentRaceChangedEventHandler CurrentRaceChanged;
        public static event NextRacesUpdatedEventHandler NextRacesUpdated;
        public static event RacesUpdatedEventHandler RacesUpdated;
        public static event StandingsUpdatedEventHandler StandingsUpdated;
        public static event TournamentsUpdatedEventHandler TournamentsUpdated;
        public static event TournamentUpdatedEventHandler TournamentUpdated;

        public static bool Simulate { get; set; } = false;
        public static string ComPort { get; set; }
        public static int BaudRate { get; set; } = 115200;

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

            TournamentsUpdated?.Invoke(new TournamentsUpdatedEventArgs(GetTournaments()));

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

            TournamentUpdated?.Invoke(new TournamentUpdatedEventArgs(tournament));

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

            CarsUpdated?.Invoke(tournamentID, new CarsUpdatedEventArgs(GetCars(tournamentID)));
            StandingsUpdated?.Invoke(tournamentID, new StandingsUpdatedEventArgs(GetStandings(tournamentID)));

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

            CarUpdated?.Invoke(tournamentID, new CarUpdatedEventArgs(updatedCar));
            StandingsUpdated?.Invoke(tournamentID, new StandingsUpdatedEventArgs(GetStandings(tournamentID)));

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

            CarsUpdated?.Invoke(tournamentID, new CarsUpdatedEventArgs(GetCars(tournamentID)));
            StandingsUpdated?.Invoke(tournamentID, new StandingsUpdatedEventArgs(GetStandings(tournamentID)));

            return deletedCar;
        }

        private static void GenerateRaces(ref Tournament tournament)
        {
            tournament.RaceData = new List<Race>();

            RaceGenerator generator = new RaceGenerator(null);

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
                    int rawRacer = rawRaces[raceNum, laneNum];
                    if (rawRacer <= 0)
                    {
                        laneAssignment.Car = null;
                    }
                    else
                    {
                        laneAssignment.Car = Car.From(cars[rawRacer - 1]);
                    }
                    race.LaneAssignmentData.Add(laneAssignment);
                }

                tournament.RaceData.Add(race);
            }

            if (rawRaces.GetLength(0) > 0)
            {
                tournament.CurrentRace = 1;
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

            RacesUpdated?.Invoke(tournamentID, new RacesUpdatedEventArgs(result));
            CurrentRaceChanged?.Invoke(tournamentID, new CurrentRaceChangedEventArgs(null, GetCurrentRace(tournamentID)));
            //RaceUpdated?.Invoke(tournamentID, new RaceUpdatedEventArgs(GetCurrentRace(tournamentID)));
            NextRacesUpdated?.Invoke(tournamentID, new NextRacesUpdatedEventArgs(GetNextRaces(tournamentID)));
            StandingsUpdated?.Invoke(tournamentID, new StandingsUpdatedEventArgs(GetStandings(tournamentID)));

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
                    if (assignment.Car == null)
                    {
                        continue;
                    }

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

                // CurrentRace starts at 1
                if ((tournament.CurrentRace >= 1) && (tournament.RaceData.Count > (tournament.CurrentRace - 1)))
                {
                    result = tournament.RaceData[tournament.CurrentRace - 1];
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

        public static double CalculateSpeed(int trackLengthInches, double seconds)
        {
            double scaleMiles = trackLengthInches / 2217.6;
            double scaleMph = (scaleMiles * 3600) / seconds;
            return scaleMph;
        }

        public static List<Standing> GetStandings(Tournament tournament, bool orderByPoints = true, string groupName = null)
        {
            List<Standing> result = new List<Standing>();

            Dictionary<int, Standing> carStandingsDictionary = new Dictionary<int, Standing>();
            Dictionary<int, List<long>> carTimes = new Dictionary<int, List<long>>();
            foreach (Car car in tournament.CarData)
            {
                Standing standing = new Standing
                {
                    Car = car,
                    Points = 0,
                    AverageTime = 0
                };
                carStandingsDictionary.Add(car.ID, standing);
                carTimes.Add(car.ID, new List<long>());
            }

            foreach (Race race in tournament.RaceData)
            {
                foreach (LaneAssignment assignment in race.LaneAssignmentData)
                {
                    if (assignment.Car != null)
                    {
                        carStandingsDictionary[assignment.Car.ID].Points += assignment.Points;
                        carTimes[assignment.Car.ID].Add(assignment.ElapsedTime);
                    }
                }
            }

            // calculate the average time for each car
            foreach (int carID in carTimes.Keys)
            {
                long sum = 0;
                foreach (long elapsedTime in carTimes[carID])
                {
                    sum += elapsedTime;
                }

                if (carTimes[carID].Count > 0)
                {
                    Standing standing = carStandingsDictionary[carID];
                    standing.AverageTime = (long)((double)sum / carTimes[carID].Count);
                    standing.AverageSpeed = CalculateSpeed(tournament.TrackLengthInches, ((double)standing.AverageTime) / 100000);
                }
            }

            List<KeyValuePair<int, Standing>> orderedStandings = null;
            if (orderByPoints)
            {
                orderedStandings = carStandingsDictionary.OrderByDescending(cp => cp.Value.Points).ToList();
            }
            else
            {
                orderedStandings = carStandingsDictionary.OrderBy(cp => cp.Value.AverageTime).ToList();
            }

            if (!String.IsNullOrEmpty(groupName))
            {
                orderedStandings = orderedStandings.Where(s => s.Value.Car.Den == groupName).ToList();
            }

            int index = 0;
            int position = 1;
            while (index < orderedStandings.Count())
            {
                orderedStandings[index].Value.Position = GetPositionText(position);

                // figure out the next position
                if ((index + 1) < orderedStandings.Count())
                {
                    // this is not the end
                    Standing current = orderedStandings[index].Value;
                    Standing next = orderedStandings[index + 1].Value;

                    if ((orderByPoints && (next.Points != current.Points)) ||
                        (!orderByPoints && (next.AverageTime != current.AverageTime)))
                    {
                        position++;
                    }
                }

                index++;
            }

            result = orderedStandings.Select(o => o.Value).ToList();

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

                // CurrentRace starts at 1
                if (tournament.CurrentRace > 0)
                {
                    if (tournament.CurrentRace < tournament.RaceData.Count)
                    {
                        result.Add(tournament.RaceData[tournament.CurrentRace]);
                    }

                    if (tournament.CurrentRace + 1 < tournament.RaceData.Count)
                    {
                        result.Add(tournament.RaceData[tournament.CurrentRace + 1]);
                    }
                }
            }

            return result;
        }

        public static void SetCurrentRace(int tournamentID, int raceNum)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            IRace previousRace = GetCurrentRace(tournamentID);

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);
                Race race = tournament.RaceData.Where(r => r.RaceNumber == raceNum).Single();
                tournament.CurrentRace = raceNum;
                repo.SaveTournament(tournament);
            }

            CurrentRaceChanged?.Invoke(tournamentID, new CurrentRaceChangedEventArgs(previousRace, GetCurrentRace(tournamentID)));
            NextRacesUpdated?.Invoke(tournamentID, new NextRacesUpdatedEventArgs(GetNextRaces(tournamentID)));
        }

        public static void StartRace(int tournamentID, int raceNum)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);
                Race race = tournament.RaceData.Where(r => r.RaceNumber == raceNum).Single();
                tournament.CurrentRace = raceNum;
                foreach (LaneAssignment assignment in race.LaneAssignmentData)
                {
                    assignment.ElapsedTime = 0;
                    assignment.Points = 0;
                    assignment.Position = 0;
                    assignment.ScaleSpeed = 0.0;
                }
                race.State = RaceState.Racing;
                repo.SaveTournament(tournament);
            }

            //RacesUpdated?.Invoke(tournamentID, new RacesUpdatedEventArgs(GetRaces(tournamentID)));
            RaceUpdated?.Invoke(tournamentID, new RaceUpdatedEventArgs(GetCurrentRace(tournamentID)));
            //NextRacesUpdated?.Invoke(tournamentID, new NextRacesUpdatedEventArgs(GetNextRaces(tournamentID)));

            RaceMonitor.LaneResultAdded += RaceMonitor_LaneResultAdded;
            RaceMonitor.Monitor(TournamentManager.ComPort, TournamentManager.BaudRate, tournamentID, raceNum, TournamentManager.Simulate);
        }

        public static void RaceMonitor_LaneResultAdded(LaneResultEventArgs e)
        {
            UpdateRaceTime(e.TournamentID, e.RaceNum, e.LaneNum, e.Time);

            //RacesUpdated?.Invoke(e.TournamentID, new RacesUpdatedEventArgs(GetRaces(e.TournamentID)));
            RaceUpdated?.Invoke(e.TournamentID, new RaceUpdatedEventArgs(GetCurrentRace(e.TournamentID)));
            StandingsUpdated?.Invoke(e.TournamentID, new StandingsUpdatedEventArgs(GetStandings(e.TournamentID)));
        }

        public static void StopRace(int tournamentID, int raceNum)
        {
            RaceMonitor.Stop();
            RaceMonitor.LaneResultAdded -= RaceMonitor_LaneResultAdded;

            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);
                Race race = tournament.RaceData.Where(r => r.RaceNumber == raceNum).Single();
                tournament.CurrentRace = raceNum;
                race.State = RaceState.Done;
                repo.SaveTournament(tournament);
            }

            //RacesUpdated?.Invoke(tournamentID, new RacesUpdatedEventArgs(GetRaces(tournamentID)));
            RaceUpdated?.Invoke(tournamentID, new RaceUpdatedEventArgs(GetCurrentRace(tournamentID)));
            //NextRacesUpdated?.Invoke(tournamentID, new NextRacesUpdatedEventArgs(GetNextRaces(tournamentID)));
        }

        public static IRace UpdateRace(int tournamentID, IRace race)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();
            Race updatedRace = null;

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);

                updatedRace = tournament.RaceData.Where(r => r.RaceNumber == race.RaceNumber).Single();

                if (updatedRace.LaneAssignmentData.Count != race.LaneAssignments.Count())
                {
                    throw new ArgumentException("Races do not have the same number of lane assignments");
                }

                updatedRace.State = race.State;

                ILaneAssignment[] laneAssignments = race.LaneAssignments.ToArray();
                for (int i = 0; i < updatedRace.LaneAssignmentData.Count; i++)
                {
                    LaneAssignment to = updatedRace.LaneAssignmentData[i];
                    ILaneAssignment from = laneAssignments[i];
                    to.ElapsedTime = from.ElapsedTime;
                    to.Points = from.Points;
                    to.Position = from.Position;
                }

                repo.SaveTournament(tournament);
            }

            //RacesUpdated?.Invoke(tournamentID, new RacesUpdatedEventArgs(GetRaces(tournamentID)));
            RaceUpdated?.Invoke(tournamentID, new RaceUpdatedEventArgs(updatedRace));
            //NextRacesUpdated?.Invoke(tournamentID, new NextRacesUpdatedEventArgs(GetNextRaces(tournamentID)));
            StandingsUpdated?.Invoke(tournamentID, new StandingsUpdatedEventArgs(GetStandings(tournamentID)));

            return updatedRace;
        }

        private static void CalculatePoints(ref Race race)
        {
            List<LaneAssignment> orderedLanes = race.LaneAssignmentData.OrderBy(a => a.ElapsedTime).ToList();
            List<LaneAssignment> dnfs = orderedLanes.Where(l => l.ElapsedTime == 0).ToList();

            // move the DNFs to the end
            foreach (LaneAssignment assignment in dnfs)
            {
                orderedLanes.Remove(assignment);
                orderedLanes.Add(assignment);
            }

            for (int position = 1; position <= orderedLanes.Count; position++)
            {
                int points = 0;
                switch (position)
                {
                    case 1:
                        points = 3;
                        break;
                    case 2:
                        points = 2;
                        break;
                    case 3:
                        points = 1;
                        break;
                    default:
                        points = 0;
                        break;
                }

                orderedLanes[position - 1].Points = points;
                orderedLanes[position - 1].Position = position;
            }
        }

        private static void UpdateRaceTime(int tournamentID, int raceNum, int lane, long elapsedTime)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);

                Race race = tournament.RaceData.Where(r => r.RaceNumber == raceNum).Single();

                if (lane > race.LaneAssignmentData.Count)
                {
                    throw new ArgumentException(string.Format("Lane {0} does not existin in race {1}", lane, race.RaceNumber));
                }

                race.LaneAssignmentData[lane - 1].ElapsedTime = elapsedTime;
                race.LaneAssignmentData[lane - 1].ScaleSpeed = CalculateSpeed(tournament.TrackLengthInches, elapsedTime / 100000.0);

                CalculatePoints(ref race);

                repo.SaveTournament(tournament);
            }
        }

        private static List<GroupResults> GenerateTournamentResults(Tournament tournament)
        {
            List<GroupResults> results = new List<GroupResults>();

            GroupResults overall = new GroupResults();
            overall.GroupName = "Overall";
            overall.Standings = GetStandings(tournament);
            results.Add(overall);

            foreach (string name in new string[] { "Lion", "Tiger", "Wolf", "Bear", "Webelos I", "Webelos II" })
            {
                GroupResults groupResult = new GroupResults();
                groupResult.GroupName = name;
                groupResult.Standings = GetStandings(tournament, false, name);
                results.Add(groupResult);
            }

            return results;
        }

        public static List<GroupResults> GetTournamentResults(int tournamentID)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();
            List<GroupResults> results = new List<GroupResults>();

            lock (_lock)
            {
                Tournament tournament = repo.LoadTournament(tournamentID);

                results = GenerateTournamentResults(tournament);
            }

            return results;
        }
    }
}
