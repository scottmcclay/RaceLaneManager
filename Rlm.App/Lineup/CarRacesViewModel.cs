using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class CarRacesViewModel
    {
        public CarViewModel Car { get; private set; }
        public CarRaceViewModel[] Races { get; private set; }

        public CarRacesViewModel(RlmGetRacesResponse.CarRaces carRaces)
        {
            this.Car = new CarViewModel(carRaces.Car);

            List<CarRaceViewModel> races = new List<CarRaceViewModel>();
            foreach (RlmGetRacesResponse.CarRaces.CarLaneAssignment assignment in carRaces.Assignments)
            {
                races.Add(new CarRaceViewModel(assignment));
            }
            this.Races = races.ToArray();
        }
    }
}
