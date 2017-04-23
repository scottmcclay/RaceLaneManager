using RaceLaneManager.Model;
using RaceLaneManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RaceLaneManager.WebApi
{
    public class RacersController : ApiController
    {
        public IEnumerable<Racer> GetAllRacers(int eventId)
        {
            RaceEvent raceEvent = RepositoryManager.GetDefaultRepository().GetEvent(eventId);
            if (raceEvent == null)
            {
                throw new ArgumentException(string.Format("Race Event with ID {0} does not exist.", eventId), "raceId");
            }

            return raceEvent.Racers;
        }

        public Racer GetRacers(int eventId, int id)
        {
            RaceEvent raceEvent = RepositoryManager.GetDefaultRepository().GetEvent(eventId);
            if (raceEvent == null)
            {
                throw new ArgumentException(string.Format("Race Event with ID {0} does not exist.", eventId), "raceId");
            }

            IEnumerable<Racer> racerMatch = raceEvent.Racers.Where(r => r.ID == id);
            if (racerMatch.Count() > 0)
            {
                return racerMatch.Single();
            }

            return null;
        }

        [HttpPost]
        public Racer CreateRacer(int eventId, [FromBody] dynamic json)
        {
            RaceEvent raceEvent = RepositoryManager.GetDefaultRepository().GetEvent(eventId);
            if (raceEvent == null)
            {
                throw new ArgumentException(string.Format("Race Event with ID {0} does not exist.", eventId), "raceId");
            }

            Racer racer = new Racer();
            if (json.Name != null)
            {
                racer.Name = json.Name.Value;
            }

            if (json.Den != null)
            {
                racer.Den = json.Den.Value;
            }

            if (json.CarNumber != null)
            {
                racer.CarNumber = (int)json.CarNumber.Value;
            }

            // get a unique ID
            int maxId = 0;
            foreach (Racer existingRacer in raceEvent.Racers)
            {
                if (existingRacer.ID > maxId)
                {
                    maxId = existingRacer.ID;
                }
            }
            racer.ID = maxId + 1;

            raceEvent.AddRacer(racer);

            return racer;
        }
    }
}
