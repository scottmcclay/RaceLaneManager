using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceLaneManager.Model
{
    public class RaceEvent
    {
        private Lane[] _lanes;
        private List<Racer> _racers = new List<Racer>();
        private List<RaceLaneAssignment> _assignments = new List<RaceLaneAssignment>();

        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public IList<Lane> Lanes { get { return _lanes; } }
        public IList<Racer> Racers { get { return _racers; } }
        public int NumRaces { get; private set; }
        public IEnumerable<RaceLaneAssignment> Assignments { get; }

        public RaceEvent(int numLanes)
        {
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

        public void AddRacer(Racer racer)
        {
            if (_assignments.Count > 0)
            {
                throw new InvalidOperationException("Race Lane Assignments must be cleared before adding users.");
            }

            _racers.Add(racer);
        }

        public Racer RemoveRacer(int racerID)
        {
            Racer racer = _racers.Where(r => r.ID == racerID).SingleOrDefault();

            if (racer != null)
            {
                _racers.Remove(racer);
            }

            return racer;
        }

        public void RemoveRacer(Racer racer)
        {
            _racers.Remove(racer);
        }
    }
}
