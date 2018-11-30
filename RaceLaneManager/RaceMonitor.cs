using Microsoft.AspNet.SignalR;
using RaceLaneManager.Model;
using RaceLaneManager.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RaceLaneManager
{
    class RaceMonitor
    {
        private static Thread _monitorThread;
        private static Random _random;
        private static SerialPort _serialPort;
        private static bool _stopMonitoring = false;

        public static void Monitor(string portName, int tournamentID, int raceNum)
        {
            if (_random == null)
            {
                _random = new Random();
            }

            // Get the SignalR Hub context
            IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<RlmHub>();

            // Open the serial port
            Debug.WriteLine("Opening serial port {0}", portName);
            _serialPort = new SerialPort(portName, 9600);
            _serialPort.ReadTimeout = 100;
            _serialPort.Open();

            // Start the monitor thread
            _stopMonitoring = false;
            _monitorThread = new Thread(() => MonitorFunction(_serialPort, tournamentID, raceNum, hub));
            _monitorThread.Start();
        }

        private static void MonitorFunction(SerialPort port, int tournamentID, int raceNum, IHubContext hub)
        {
            Debug.WriteLine("Monitor thread started");

            port.DiscardInBuffer();

            while (!_stopMonitoring)
            {
                try
                {
                    string message = port.ReadLine();
                    Debug.WriteLine(message);

                    Regex r = new Regex(@"L(?<laneNum>\d) (?<time>\d+)");
                    Match m = r.Match(message);
                    if (m.Success)
                    {
                        // update the race
                        TournamentManager.UpdateRaceTime(tournamentID, raceNum, int.Parse(m.Groups["laneNum"].Value), long.Parse(m.Groups["time"].Value));

                        // notify clients
                        hub.Clients.All.racesUpdated(tournamentID, TournamentManager.GetRaces(tournamentID));
                        hub.Clients.All.currentRaceUpdated(tournamentID, TournamentManager.GetCurrentRace(tournamentID));
                        hub.Clients.All.standingsUpdated(tournamentID, TournamentManager.GetStandings(tournamentID));
                    }
                }
                catch (TimeoutException)
                {
                    // nothing wrong with a timeout, just continue
                }
            }

            Debug.WriteLine("Monitor thread ending");
        }

        public static void Stop()
        {
            if (_monitorThread != null)
            {
                Debug.WriteLine("Signaling monitor thread to end");
                _stopMonitoring = true;
                try
                {
                    _monitorThread.Join(500);
                    Debug.WriteLine("Detected termination of monitor thread");
                }
                catch (TimeoutException ex)
                {
                    Debug.WriteLine("Unable to terminate monitor thread - {0}", ex.Message);
                }
                _monitorThread = null;

                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
                _serialPort.Dispose();
                _serialPort = null;
            }
        }

        private static void SimulateRace(int tournamentID, int raceNum, IHubContext hub)
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
    }
}
