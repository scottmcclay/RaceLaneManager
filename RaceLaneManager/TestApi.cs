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
        public IList<Car> Racer()
        {
            IList<Car> racers = new List<Car>();

            racers.Add(new Car() { Owner = "Joseph", Den = "Wolf", CarNumber = 2 });
            racers.Add(new Car() { Owner = "Brenden", Den = "Wolf", CarNumber = 1 });
            racers.Add(new Car() { Owner = "Owen", Den = "Wolf", CarNumber = 3 });
            racers.Add(new Car() { Owner = "Adam", Den = "Wolf", CarNumber = 4 });

            return racers;
        }
    }
}
