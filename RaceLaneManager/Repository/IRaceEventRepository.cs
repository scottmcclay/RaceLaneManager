using RaceLaneManager.Model;
using System.Collections.Generic;

namespace RaceLaneManager.Repository
{
    public interface IRaceEventRepository
    {
        IList<RaceEvent> GetAllEvents();
        RaceEvent GetEvent(int eventId);
        RaceEvent AddEvent(RaceEvent raceEvent);
        void RemoveEvent(int eventId);
        bool SaveEvent(int eventId);
    }
}
