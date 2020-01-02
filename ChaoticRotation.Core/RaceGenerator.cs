//
// RaceGenerator.cs
//
// Authors:
//  Scott McClay <scott.mcclay.csm@gmail.com>
//
// Copyright (C) 2017 Scott McClay
//
// You may use, distribute and modify this code under the terms of the
// GNU Affero General Public License Verson 3 (GNU AGPL V3).
// http://www.gnu.org/licenses/agpl.html
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ChaoticRotation.Core
{
    public class RaceGenerator
    {
        private ILogger _logger;

        private static Random _rand = new Random();

        public int MinSteps { get; set; }
        public int ActualSteps { get; set; }
        public int Backtracks { get; set; }

        public RaceGenerator(ILogger logger)
        {
            _logger = logger;
        }

        public int GetNumRaces(int numRacers, int numLanes)
        {
            int numRaces = numRacers;
            if (numRaces < numLanes)
            {
                numRaces = numLanes;
            }

            return numRaces;
        }

        public int[,] Solve(int numRacers, int numLanes)
        {
            int numRaces = GetNumRaces(numRacers, numLanes);
            int[,] result = new int[numRaces, numLanes];

            this.MinSteps = numRacers * numLanes;

            int ghostRacers = 0;
            int totalRacers = numRacers;
            if (numRacers < numLanes)
            {
                ghostRacers = numLanes - numRacers;
                totalRacers = numLanes;
            }

            // start with Race 1, Lane 1
            if (Solve(ref result, totalRacers, numLanes, numRaces, 1, 1) == false)
            {
                throw new Exception("Could not solve problem.");
            }

            if (totalRacers > numRacers)
            {
                for (int race = 0; race < numRaces; race++)
                {
                    for (int lane = 0; lane < numLanes; lane++)
                    {
                        if (result[race, lane] > numRacers)
                        {
                            result[race, lane] = -1;
                        }
                    }
                }
            }

            return result;
        }

        public bool Solve(ref int[,] races, int numRacers, int numLanes, int numRaces, int raceNum, int laneNum)
        {
            // get a randomized list of candidates for this lane in this race
            IList<int> candidates = GetCandidates(ref races, numRacers, numLanes, raceNum, laneNum);

            if (candidates.Count() <= 0)
            {
                // no available candidates
                _logger?.LogMessage(this, $"No candidates available for race {raceNum} lane {laneNum} - backtracking");
                return false;
            }

            int racersAttempted = 0;
            do
            {
                // assign a racer
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(string.Format("Candidates for race {0} lane {1}: ", raceNum, laneNum));
                foreach (int candidate in candidates) { sb.AppendFormat(" {0}", candidate); }
                _logger?.LogMessage(this, sb.ToString());

                int racer = candidates[racersAttempted];
                _logger?.LogMessage(this, $"Race {raceNum}: assigning racer {racer} to lane {laneNum}");
                races[raceNum - 1, laneNum - 1] = racer;

                this.ActualSteps++;

                // see if the remaining lane assignments can be made
                int nextLaneNum = laneNum + 1;
                int nextRaceNum = raceNum;
                if (nextLaneNum > numLanes)
                {
                    nextLaneNum = 1;
                    nextRaceNum++;
                }

                if (nextRaceNum > numRaces)
                {
                    // all lane assignments have been made, success - stop
                    return true;
                }

                if (Solve(ref races, numRacers, numLanes, numRaces, nextRaceNum, nextLaneNum) == true)
                {
                    // a solution was found using this lane assignment
                    return true;
                }

                // can't solve the problem with the selected racer, try the next candidate
                _logger?.LogMessage(this, $"Race {raceNum}: Racer {racer} in lane {laneNum} did not work");
                this.Backtracks++;

                racersAttempted++;
            } while (racersAttempted < candidates.Count());

            // all candidates have been exhausted and a solution was not found
            _logger?.LogMessage(this, $"No more candidates available for race {raceNum} lane {laneNum} - backtracking");
            return false;
        }

        public IList<int> GetCandidates(ref int[,] races, int numRacers, int numLanes, int raceNum, int laneNum)
        {
            List<int> result = new List<int>();

            IList<RacerAssignmentState> racerStates = GetRacerAssignmentStates(races, numRacers, numLanes, raceNum, laneNum);

            List<RacerAssignmentState> availableForLane = racerStates
                .Where(r => r.RaceAssignments[laneNum - 1] == 0)      // racers that haven't been assigned to the lane
                .Where(r => !r.RaceAssignments.Contains(raceNum)) // racers that aren't in the race already
                .ToList();

            if (availableForLane.Count == 0)
            {
                // no viable candidates
                return result;
            }

            // Prioritize racers with fewer assigned races than others
            int minRaceAssignments = availableForLane.Select(r => r.AssignedRaces).Min();
            List<RacerAssignmentState> minAssignmentRacers = availableForLane.Where(r => r.AssignedRaces == minRaceAssignments).ToList();

            // Favor racers that have not been assigned a recent race, to reduce their wait time between races
            //int minLastRace = minAssignmentRacers.Select(r => r.LastRace).Min();

            //List<RacerAssignmentState> candidates = minAssignmentRacers.Where(r => r.LastRace == minLastRace).ToList();

            //result.AddRange(candidates.Select(c => c.Number).ToList());
            result.AddRange(minAssignmentRacers.Select(c => c.Number).ToList());
            result.Randomize(_rand);
            return result;
        }

        /// <summary>
        /// Scan all the existing assignments and identify how many races each racer is assigned to and which lanes the
        /// racer still needs to race on. This assumes lane assignments have been made in the following order:
        ///   Race 1, Lane 1
        ///   Race 1, Lane 2
        ///   ...
        ///   Race 2, Lane 1
        ///   Race 2, Lane 2
        ///   ...
        /// </summary>
        /// <param name="races">2-D array of races x lanes. Assignments must have non-zero values.</param>
        /// <param name="numRacers">The number of racers</param>
        /// <param name="numLanes">The number of lanes</param>
        /// <param name="raceNum">The race that needs a lane assignment</param>
        /// <param name="laneNum">The lane that is needing assigmnet</param>
        /// <returns>A list of RacerAssignmentStates and accounts for all existing racer/lane/race assigments</returns>
        private IList<RacerAssignmentState> GetRacerAssignmentStates(int[,] races, int numRacers, int numLanes, int raceNum, int laneNum)
        {
            List<RacerAssignmentState> result = new List<RacerAssignmentState>();

            for (int i = 1; i <= numRacers; i++)
            {
                result.Add(new RacerAssignmentState(i, numLanes));
            }

            for (int race = 1; race <= raceNum; race++)
            {
                // check all lanes for previous races but only up to (not including) current lane for current race
                int laneMax = numLanes;
                if (race == raceNum)
                {
                    laneMax = laneNum - 1;
                }

                for (int lane = 1; lane <= laneMax; lane++)
                {
                    int racer = races[race - 1, lane - 1];
                    if (racer != -1)
                    {
                        result[racer - 1].AssignRace(race, lane);
                    }
                }
            }

            return result;
        }

        private class RacerAssignmentState
        {
            public int Number { get; private set; }

            private int[] _raceAssignmentsByLane;
            public int AssignedRaces { get; private set; }
            public int LastRace { get; private set; }

            public IList<int> RaceAssignments
            {
                get { return _raceAssignmentsByLane; }
            }

            public RacerAssignmentState(int number, int numLanes)
            {
                this.Number = number;
                _raceAssignmentsByLane = new int[numLanes];
            }

            public void AssignRace(int raceNum, int laneNum)
            {
                _raceAssignmentsByLane[laneNum - 1] = raceNum;

                this.AssignedRaces = _raceAssignmentsByLane.Where(a => a != 0).Count();
                this.LastRace = _raceAssignmentsByLane.Max();
            }
        }
    }

    public static class ListHelper
    {
        public static void Randomize<T>(this IList<T> list, Random rand)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
