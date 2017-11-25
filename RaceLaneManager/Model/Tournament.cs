using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceLaneManager.Model
{
    public class Tournament
    {
        private List<Car> _cars = new List<Car>();
        private List<RaceLaneAssignment> _assignments = new List<RaceLaneAssignment>();

        public int ID { get; set; }
        public string Name { get; set; }
        public int NumLanes { get; set; }
        public IList<Car> Cars { get { return _cars; } }
        public int NumRaces { get; private set; }
        public IEnumerable<RaceLaneAssignment> Assignments { get; }

        public Tournament(string name, int numLanes)
        {
            this.Name = name;
            this.NumLanes = numLanes;
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
            if (_assignments.Count > 0)
            {
                throw new InvalidOperationException("Race Lane Assignments must be cleared before removing cars.");
            }

            Car car = _cars.Where(r => r.ID == carId).SingleOrDefault();

            if (car != null)
            {
                _cars.Remove(car);
            }

            return car;
        }

        public void RemoveCar(Car car)
        {
            if (_assignments.Count > 0)
            {
                throw new InvalidOperationException("Race Lane Assignments must be cleared before removing cars.");
            }

            _cars.Remove(car);
        }
    }
}
