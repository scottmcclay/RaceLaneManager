using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceLaneManager.Repository;

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
    }
}
