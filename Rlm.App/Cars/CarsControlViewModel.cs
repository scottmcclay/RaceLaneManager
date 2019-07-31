using MaterialDesignThemes.Wpf;
using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class CarsControlViewModel
    {
        public int TournamentID { get; private set; }

        private ObservableCollection<CarViewModel> _cars = new ObservableCollection<CarViewModel>();
        public ObservableCollection<CarViewModel> Cars => _cars;

        public CarsControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;

            PopulateCars(TournamentManager.GetCars(this.TournamentID));

            TournamentManager.CarsUpdated += TournamentManager_CarsUpdated;
            TournamentManager.CarUpdated += TournamentManager_CarUpdated;
        }

        private void PopulateCars(IEnumerable<ICar> cars)
        {
            this.Cars.Clear();
            foreach (ICar car in cars)
            {
                _cars.Add(new CarViewModel(car));
            }
        }

        private void TournamentManager_CarUpdated(int tournamentID, CarUpdatedEventArgs e)
        {
            PopulateCars(TournamentManager.GetCars(this.TournamentID));
        }

        private void TournamentManager_CarsUpdated(int tournamentID, CarsUpdatedEventArgs e)
        {
            PopulateCars(e.Cars);
        }

        public void AddCar(ICar car)
        {
            TournamentManager.AddCar(this.TournamentID, car);
        }

        public void UpdateCar(ICar car)
        {
            TournamentManager.UpdateCar(this.TournamentID, car);
        }

        public void DeleteCar(ICar car)
        {
            TournamentManager.DeleteCar(this.TournamentID, car.ID);
        }
    }
}
