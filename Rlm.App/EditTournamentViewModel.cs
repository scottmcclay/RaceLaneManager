using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    public class EditTournamentViewModel : PropertyChangeNotifier
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(this.Name));
                }
            }
        }

        private int _numLanes;
        public int NumLanes
        {
            get => _numLanes;
            set
            {
                if (_numLanes != value)
                {
                    _numLanes = value;
                    OnPropertyChanged(nameof(this.NumLanes));
                }
            }
        }

        public EditTournamentViewModel(ITournament tournament)
        {
            _name = tournament.Name;
            _numLanes = tournament.NumLanes;
        }
    }
}
