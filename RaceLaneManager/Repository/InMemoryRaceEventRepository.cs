using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceLaneManager.Model;

namespace RaceLaneManager.Repository
{
    public class InMemoryRaceEventRepository : IRaceEventRepository
    {
        private static List<RaceEvent> _events = new List<RaceEvent>();

        public IList<RaceEvent> GetAllEvents()
        {
            return _events;
        }

        public RaceEvent GetEvent(int eventId)
        {
            return _events.Where(e => e.ID == eventId).Single();
        }

        public RaceEvent AddEvent(RaceEvent raceEvent)
        {
            // find an ID to assign to this new RaceEvent
            int maxID = 0;
            foreach (RaceEvent r in _events)
            {
                if (r.ID > maxID)
                {
                    maxID = r.ID;
                }
            }

            raceEvent.ID = maxID + 1;
            _events.Add(raceEvent);

            return raceEvent;
        }

        public bool SaveEvent(int eventId)
        {
            return true;
        }

        public void RemoveEvent(int eventId)
        {
            RaceEvent raceEvent = _events.Where(e => e.ID == eventId).Single();
            if (raceEvent == null)
            {
                throw new ArgumentException(string.Format("RaceEvent with ID {0} was not found.", eventId), "eventId");
            }

            _events.Remove(raceEvent);
        }
    }
}
