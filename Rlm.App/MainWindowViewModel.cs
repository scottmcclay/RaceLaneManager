using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rlm.Core;

namespace Rlm.App
{
    class MainWindowViewModel : PropertyChangeNotifier
    {
        public ObservableCollection<TournamentViewModel> Tournaments { get; } = new ObservableCollection<TournamentViewModel>();

        public MainWindowViewModel()
        {
            TournamentManager.TournamentsUpdated += TournamentManager_TournamentsUpdated;
            AddTournaments(TournamentManager.GetTournaments());
        }

        private void AddTournaments(IEnumerable<ITournament> tournaments)
        {
            foreach (ITournament tournament in tournaments)
            {
                this.Tournaments.Add(new TournamentViewModel(tournament));
            }
        }

        private void TournamentManager_TournamentsUpdated(TournamentsUpdatedEventArgs e)
        {
            this.Tournaments.Clear();
            AddTournaments(e.Tournaments);
        }
    }
}
