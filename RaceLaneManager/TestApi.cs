using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RaceLaneManager.Model
{
    public class TestApiController : ApiController
    {
        [HttpGet]
        public IList<Racer> Racer()
        {
            IList<Racer> racers = new List<Racer>();

            racers.Add(new Racer() { Name = "Joseph", Den = "Wolf", CarNumber = 2 });
            racers.Add(new Racer() { Name = "Brenden", Den = "Wolf", CarNumber = 1 });
            racers.Add(new Racer() { Name = "Owen", Den = "Wolf", CarNumber = 3 });
            racers.Add(new Racer() { Name = "Adam", Den = "Wolf", CarNumber = 4 });

            return racers;
        }
    }
}
