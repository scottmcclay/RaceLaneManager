using Microsoft.AspNet.SignalR;
using RaceLaneManager.Model;
using RaceLaneManager.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaceLaneManager
{
    class RaceMonitor
    {
        private static Thread _monitorThread;
        private static Random _random;

        public static void Monitor(int tournamentID, int raceNum)
        {
            if (_random == null)
            {
                _random = new Random();
            }

            // Get the SignalR Hub context
            IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<RlmHub>();

            // Start the monitor thread
            _monitorThread = new Thread(() => MonitorFunction(tournamentID, raceNum, hub));
            _monitorThread.Start();
        }

        private static void MonitorFunction(int tournamentID, int raceNum, IHubContext hub)
        {
            // generate simulation
            Dictionary<int, int> simLaneTimes = new Dictionary<int, int>();

            ITournament tournament = TournamentManager.GetTournament(tournamentID);
            for (int i = 0; i < tournament.NumLanes; i++)
            {
                simLaneTimes.Add(i + 1, _random.Next(4500, 6000));
            }

            // identify the sleep times
            List<KeyValuePair<int, int>> orderedLaneTimes = simLaneTimes.OrderBy(p => p.Value).ToList();
            List<int> sleepTimes = new List<int>();
            int elapsedTime = 0;
            for (int i = 0; i < orderedLaneTimes.Count; i++)
            {
                sleepTimes.Add(orderedLaneTimes[i].Value - elapsedTime);
                elapsedTime = orderedLaneTimes[i].Value;
            }

            // simulate a delay in pressing the start button
            Thread.Sleep(_random.Next(500, 3000));

            for (int i = 0; i < orderedLaneTimes.Count; i++)
            {
                // sleep until the car crosses the finish line
                Thread.Sleep(sleepTimes[i]);

                // simulate DNF
                int dnf = _random.Next(1, 20);
                if (dnf != 7) // 7 is random - just one number out of 20 which should be hit about 5% of the time
                {
                    // only update the race time if this is NOT a DNF

                    // update the race
                    TournamentManager.UpdateRaceTime(tournamentID, raceNum, orderedLaneTimes[i].Key, orderedLaneTimes[i].Value);

                    // notify clients
                    hub.Clients.All.racesUpdated(tournamentID, TournamentManager.GetRaces(tournamentID));
                    hub.Clients.All.currentRaceUpdated(tournamentID, TournamentManager.GetCurrentRace(tournamentID));
                    hub.Clients.All.standingsUpdated(tournamentID, TournamentManager.GetStandings(tournamentID));
                }
            }

            while (true)
            {
                Thread.Sleep(500);
            }
        }

        public static void Stop()
        {
            if (_monitorThread != null)
            {
                _monitorThread.Abort();
                _monitorThread = null;
            }
        }
    }
}
