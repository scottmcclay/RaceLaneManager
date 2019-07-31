using Rlm.Core;

namespace Rlm.App
{
    class TournamentControlViewModel : PropertyChangeNotifier
    {
        private Tournament _tournament;

        public string Name
        {
            get { return (_tournament == null ? "Unknown" : _tournament.Name); }
        }

        private RaceDetailsControlViewModel _currentRace;
        public RaceDetailsControlViewModel CurrentRace
        {
            get { return _currentRace; }
            set
            {
                if (_currentRace != value)
                {
                    _currentRace = value;
                    OnPropertyChanged(nameof(this.CurrentRace));
                }
            }
        }

        public TournamentControlViewModel()
        {
            new RaceDetailsControlViewModel();
        }

        public TournamentControlViewModel(Tournament tournament)
        {
            _tournament = tournament;
        }
    }
}
