using RaceLaneManager.Model;
using RaceLaneManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RaceLaneManager.WebApi
{
    public class EventsController : ApiController
    {
        public IEnumerable<RaceEvent> GetAllEvents()
        {
            return RepositoryManager.GetDefaultRepository().GetAllEvents();
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage CreateEvent([FromBody] dynamic json)
        {
            int lanes = (int)json.lanes;

            RaceEvent raceEvent = new RaceEvent(lanes);
            raceEvent.Date = DateTime.Now;

            RaceEvent newEvent = RepositoryManager.GetDefaultRepository().AddEvent(raceEvent);
            HttpResponseMessage response = Request.CreateResponse<RaceEvent>(HttpStatusCode.Created, newEvent);
            string uri = Url.Link("RaceLaneManagerEventApi", new { id = newEvent.ID });
            response.Headers.Location = new Uri(uri);

            return response;
        }

        public RaceEvent GetEvent(int eventId)
        {
            return RepositoryManager.GetDefaultRepository().GetEvent(eventId);
        }
    }
}
