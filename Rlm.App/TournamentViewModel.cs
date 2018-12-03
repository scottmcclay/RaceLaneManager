using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rlm.Core;

namespace Rlm.App
{
    class TournamentViewModel : PropertyChangeNotifier
    {
        private ITournament _tournament;
        public ITournament Tournament => _tournament;

        public int TournamentID
        {
            get { return _tournament == null ? -1 : _tournament.ID; }
        }

        public string Name
        {
            get { return _tournament?.Name; }
        }

        public TournamentControl TournamentControl { get; private set; }

        public TournamentViewModel(ITournament tournament)
        {
            _tournament = tournament;
            TournamentManager.TournamentUpdated += TournamentManager_TournamentUpdated;
            this.TournamentControl = new TournamentControl();
        }

        private void TournamentManager_TournamentUpdated(TournamentUpdatedEventArgs e)
        {
            _tournament = e.Tournament;
            OnPropertyChanged(nameof(this.Name));
        }
    }
}
