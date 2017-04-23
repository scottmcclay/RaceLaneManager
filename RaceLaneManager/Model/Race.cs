using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceLaneManager.Model
{
    public class Race
    {
        private RaceLaneAssignment[] _assignments;

        public IList<RaceLaneAssignment> Assignments { get { return _assignments; } }

        public Race(int numLanes)
        {
            _assignments = new RaceLaneAssignment[numLanes];
        }

        public void AssignLanes(IList<RaceLaneAssignment> assignments)
        {
            if (assignments.Count() != _assignments.Length)
            {
                throw new ArgumentException(
                    string.Format("Number of assignments ({0}) does not match the number of lanes ({1})",
                        assignments.Count(),
                        _assignments.Length),
                    "assignments");
            }

            for (int i = 0; i < _assignments.Length; i++)
            {
                _assignments[i] = assignments[i];
            }
        }
    }
}
