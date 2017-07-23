using RaceLaneManager.Model;
using RaceLaneManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RaceLaneManager.WebApi
{
    public class CarsController : ApiController
    {
        public IEnumerable<Car> GetAllCars(int tournamentId)
        {
            Tournament tournament = RepositoryManager.GetDefaultRepository().GetTournament(tournamentId);
            if (tournament == null)
            {
                throw new ArgumentException(string.Format("Tournament with ID {0} does not exist.", tournamentId), "tournamentId");
            }

            return tournament.Cars;
        }

        public Car GetCar(int tournamentId, int id)
        {
            Tournament tournament = RepositoryManager.GetDefaultRepository().GetTournament(tournamentId);
            if (tournament == null)
            {
                throw new ArgumentException(string.Format("Tournament with ID {0} does not exist.", tournamentId), "tournamentId");
            }

            IEnumerable<Car> carMatch = tournament.Cars.Where(c => c.ID == id);
            if (carMatch.Count() > 0)
            {
                return carMatch.Single();
            }

            return null;
        }

        public Car PostCar(int tournamentId, [FromBody] dynamic json)
        {
            Tournament tournament = RepositoryManager.GetDefaultRepository().GetTournament(tournamentId);
            if (tournament == null)
            {
                throw new ArgumentException(string.Format("Tournament with ID {0} does not exist.", tournamentId), "tournamentId");
            }

            Car car = new Car();
            if (json.Owner != null)
            {
                car.Owner = json.Owner.Value;
            }

            if (json.Den != null)
            {
                car.Den = json.Den.Value;
            }

            if (json.CarNumber != null)
            {
                car.CarNumber = (int)json.CarNumber.Value;
            }

            // get a unique ID
            int maxId = 0;
            foreach (Car existingCar in tournament.Cars)
            {
                if (existingCar.ID > maxId)
                {
                    maxId = existingCar.ID;
                }
            }
            car.ID = maxId + 1;

            tournament.AddCar(car);

            return car;
        }

        public void PutCar(int tournamentId, int id, [FromBody] dynamic json)
        {

        }

        public void DeleteCar(int tournamentId, int id)
        {

        }
    }
}
