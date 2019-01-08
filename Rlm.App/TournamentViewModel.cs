using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Rlm.Core;

namespace Rlm.App
{
    class TournamentViewModel : PropertyChangeNotifier
    {
        private ITournament _tournament;
        public ITournament Tournament => _tournament;

        private ObservableCollection<TournamentViewViewModel> _views = new ObservableCollection<TournamentViewViewModel>();
        public ObservableCollection<TournamentViewViewModel> Views => _views;

        public int TournamentID
        {
            get { return _tournament == null ? -1 : _tournament.ID; }
        }

        public string Name => _tournament?.Name;

        public TournamentViewModel(ITournament tournament)
        {
            _tournament = tournament;
            TournamentManager.TournamentUpdated += TournamentManager_TournamentUpdated;

            _views.Add(new TournamentViewViewModel("Home", new HomeControl
            {
                DataContext = new HomeControlViewModel(_tournament.ID)
            }));

            _views.Add(new TournamentViewViewModel("Cars", new CarsControl
            {
                DataContext = new CarsControlViewModel(_tournament.ID)
            }));

            _views.Add(new TournamentViewViewModel("Lineup", new LineupControl
            {
                DataContext = new LineupControlViewModel(_tournament.ID)
            }));

            _views.Add(new TournamentViewViewModel("Results", new ResultsControl
            {
                DataContext = new ResultsControlViewModel(_tournament.ID)
            }));
        }

        private void TournamentManager_TournamentUpdated(TournamentUpdatedEventArgs e)
        {
            if (e.Tournament.ID == this.Tournament.ID)
            {
                _tournament = e.Tournament;
                OnPropertyChanged(nameof(this.Name));
            }
        }

        public void EditTournament()
        {
            EditTournamentViewModel editVm = new EditTournamentViewModel(this.Tournament);
            DialogHost.Show(editVm, "RootDialog", EditTournamentDialogClosingEventHandler);
        }

        private void EditTournamentDialogClosingEventHandler(object sender, DialogClosingEventArgs e)
        {
            if ((e.Parameter as bool?) == true)
            {
                EditTournamentViewModel vm = e.Content as EditTournamentViewModel;
                TournamentManager.UpdateTournament(this.Tournament.ID, vm.Name, vm.NumLanes);
            }
        }
    }
}
