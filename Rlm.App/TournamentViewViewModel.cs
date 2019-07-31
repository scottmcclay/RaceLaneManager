using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rlm.App
{
    class TournamentViewViewModel
    {
        public string Name { get; set; }
        public UserControl Control { get; private set; }

        public TournamentViewViewModel(string name, UserControl control)
        {
            this.Name = name;
            this.Control = control;
        }
    }
}
