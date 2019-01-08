using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Rlm.Core
{
    public class RaceMonitor
    {
        public static LaneResultEventHandler LaneResultAdded;

        private static ILogger _logger;
        private static Thread _monitorThread;
        private static Random _random;
        private static SerialPort _serialPort;
        private static bool _stopMonitoring = false;

        public static void Monitor(string portName, int baudRate, int tournamentID, int raceNum, bool simulate = false)
        {
            if (_random == null)
            {
                _random = new Random();
            }

            if (!simulate)
            {
                // Open the serial port
                _logger?.LogMessage($"Opening serial port {portName}");
                _serialPort = new SerialPort(portName, baudRate);
                _serialPort.ReadTimeout = 100;
                _serialPort.Open();
            }

            // Start the monitor thread
            _stopMonitoring = false;
            if (simulate)
            {
                _monitorThread = new Thread(() => SimulateRace(tournamentID, raceNum));
            }
            else
            {
                _monitorThread = new Thread(() => MonitorFunction(_serialPort, tournamentID, raceNum));
            }

            _monitorThread.Start();
        }

        private static void MonitorFunction(SerialPort port, int tournamentID, int raceNum)
        {
            _logger?.LogMessage("Monitor thread started");

            port.DiscardInBuffer();

            while (!_stopMonitoring)
            {
                try
                {
                    string message = port.ReadLine();
                    _logger?.LogMessage(message);

                    Regex r = new Regex(@"L(?<laneNum>\d) (?<time>\d{6})");
                    Match m = r.Match(message);
                    if (m.Success)
                    {
                        LaneResultAdded?.Invoke(new LaneResultEventArgs(tournamentID, raceNum, int.Parse(m.Groups["laneNum"].Value) + 1, long.Parse(m.Groups["time"].Value)));
                    }
                }
                catch (TimeoutException)
                {
                    // nothing wrong with a timeout, just continue
                }
            }

            _logger?.LogMessage("Monitor thread ending");
        }

        public static void Stop()
        {
            if (_monitorThread != null)
            {
                _logger?.LogMessage("Signaling monitor thread to end");
                _stopMonitoring = true;
                try
                {
                    _monitorThread.Join(500);
                    _logger?.LogMessage("Detected termination of monitor thread");
                }
                catch (TimeoutException ex)
                {
                    _logger?.LogMessage("Unable to terminate monitor thread - {0}", ex.Message);
                }
                _monitorThread = null;

                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
                    _serialPort.Dispose();
                    _serialPort = null;
                }
            }
        }

        private static void SimulateRace(int tournamentID, int raceNum)
        {
            // generate simulation
            Dictionary<int, int> simLaneTimes = new Dictionary<int, int>();

            ITournament tournament = TournamentManager.GetTournament(tournamentID);
            for (int i = 0; i < tournament.NumLanes; i++)
            {
                simLaneTimes.Add(i + 1, _random.Next(350000, 500000));
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
                Thread.Sleep(sleepTimes[i] / 100);

                // simulate DNF
                int dnf = _random.Next(1, 20);
                if (dnf != 7) // 7 is random - just one number out of 20 which should be hit about 5% of the time
                {
                    // only update the race time if this is NOT a DNF

                    // update the race
                    LaneResultAdded?.Invoke(new LaneResultEventArgs(tournamentID, raceNum, orderedLaneTimes[i].Key, orderedLaneTimes[i].Value));
                }
            }

            while (!_stopMonitoring)
            {
                Thread.Sleep(500);
            }
        }
    }
}
