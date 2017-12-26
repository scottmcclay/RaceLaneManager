using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceLaneManager.Model;

namespace RaceLaneManager.DTOs
{
    public class RlmGetRacesResponse
    {
        public class CarRaces
        {
            public class CarLaneAssignment
            {
                public int RaceNum { get; set; }
                public int LaneNum { get; set; }
            }

            public ICar Car { get; set; }
            public List<CarLaneAssignment> Assignments { get; set; }
        }

        public int NumLanes { get; set; }
        public int NumCars { get; set; }
        public int NumRaces { get; set; }

        public IEnumerable<IRace> Races { get; set; }
        public IEnumerable<CarRaces> RacesByCar { get; set; }
    }
}
