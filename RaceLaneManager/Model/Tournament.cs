using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceLaneManager.Model
{
    public class Tournament
    {
        private Lane[] _lanes;
        private List<Car> _cars = new List<Car>();
        private List<RaceLaneAssignment> _assignments = new List<RaceLaneAssignment>();

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public IList<Lane> Lanes { get { return _lanes; } }
        public int NumLanes { get { return _lanes.Length; } }
        public IList<Car> Cars { get { return _cars; } }
        public int NumRaces { get; private set; }
        public IEnumerable<RaceLaneAssignment> Assignments { get; }

        public Tournament(string name, int numLanes)
        {
            this.Name = name;
            _lanes = new Lane[numLanes];

            for (int i = 0; i < numLanes; i++)
            {
                _lanes[i] = new Lane(i + 1, true);
            }
        }

        public void ClearAssignments()
        {
            _assignments.Clear();
        }

        public void AddCar(Car car)
        {
            if (_assignments.Count > 0)
            {
                throw new InvalidOperationException("Race Lane Assignments must be cleared before adding cars.");
            }

            _cars.Add(car);
        }

        public Car RemoveCar(int carId)
        {
            Car car = _cars.Where(r => r.ID == carId).SingleOrDefault();

            if (car != null)
            {
                _cars.Remove(car);
            }

            return car;
        }

        public void RemoveCar(Car car)
        {
            _cars.Remove(car);
        }
    }
}
