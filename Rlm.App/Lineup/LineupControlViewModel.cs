using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rlm.App
{
    class LineupControlViewModel
    {
        public event EventHandler DataChanged;

        public int TournamentID { get; private set; }
        public int NumLanes { get; private set; }
        public string[] LaneNames { get; private set; }

        public int NumRaces { get; private set; }
        public string[] RaceNames { get; private set; }

        public int NumCars { get; private set; }
        public string[] CarNames { get; private set; }

        public List<RaceViewModel> Races { get; private set; }
        public List<CarRacesViewModel> Cars { get; private set; }

        public LineupControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;
            this.Races = new List<RaceViewModel>();
            this.Cars = new List<CarRacesViewModel>();

            RlmGetRacesResponse races = TournamentManager.GetRaces(tournamentID);
            TournamentManager.RacesUpdated += TournamentManager_RacesUpdated;
            UpdateRaces(races);
        }

        private void TournamentManager_RacesUpdated(int tournamentID, RacesUpdatedEventArgs e)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => this.TournamentManager_RacesUpdated(tournamentID, e));
            }
            else
            {
                if (tournamentID != this.TournamentID) return;

                UpdateRaces(e.Races);
            }
        }

        private void UpdateRaces(RlmGetRacesResponse races)
        {
            this.NumLanes = races.NumLanes;
            this.LaneNames = GetLaneNames(this.NumLanes);

            this.NumRaces = races.NumRaces;
            this.RaceNames = GetRaceNames(this.NumRaces);

            this.NumCars = races.NumCars;
            this.CarNames = GetCarNames(races.RacesByCar);

            this.Races.Clear();
            foreach (IRace race in races.Races)
            {
                this.Races.Add(new RaceViewModel(race));
            }

            this.Cars.Clear();
            foreach (RlmGetRacesResponse.CarRaces carRaces in races.RacesByCar)
            {
                this.Cars.Add(new CarRacesViewModel(carRaces));
            }

            this.DataChanged?.Invoke(this, new EventArgs());
        }

        private string[] GetLaneNames(int numLanes)
        {
            List<string> result = new List<string>();

            for (int i = 1; i <= numLanes; i++)
            {
                result.Add($"Lane {i}");
            }

            return result.ToArray();
        }

        private string[] GetRaceNames(int numRaces)
        {
            List<string> result = new List<string>();

            for (int i = 1; i <= numRaces; i++)
            {
                result.Add($"Race {i}");
            }

            return result.ToArray();
        }

        private string[] GetCarNames(IEnumerable<RlmGetRacesResponse.CarRaces> racesByCar)
        {
            List<string> carNames = new List<string>();
            foreach (RlmGetRacesResponse.CarRaces carRaces in racesByCar)
            {
                string name = carRaces.Car.Name;
                if (string.IsNullOrEmpty(name))
                {
                    name = carRaces.Car.Owner;
                }
                carNames.Add(name);
            }
            return carNames.ToArray();
        }

        public void GenerateRaces()
        {
            TournamentManager.GenerateRaces(this.TournamentID);
        }
    }
}
