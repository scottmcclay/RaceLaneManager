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

            racers.Add(new Car() { Owner = "Joseph", Den = "Wolf" });
            racers.Add(new Car() { Owner = "Brenden", Den = "Wolf" });
            racers.Add(new Car() { Owner = "Owen", Den = "Wolf" });
            racers.Add(new Car() { Owner = "Adam", Den = "Wolf" });

            return racers;
        }
    }
}
