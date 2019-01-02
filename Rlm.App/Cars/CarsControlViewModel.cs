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

            PopulateCars();

            TournamentManager.CarsUpdated += TournamentManager_CarsUpdated;
            TournamentManager.CarUpdated += TournamentManager_CarUpdated;
        }

        private void PopulateCars()
        {
            foreach (ICar car in TournamentManager.GetCars(this.TournamentID))
            {
                _cars.Add(new CarViewModel(car));
            }
        }

        private void TournamentManager_CarUpdated(int tournamentID, CarUpdatedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TournamentManager_CarsUpdated(int tournamentID, CarsUpdatedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
