using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class StandingViewModel
    {
        private IStanding _standing;

        public string Position => _standing.Position;
        public string Car => _standing?.Car.Name;
        public string Points => $"{_standing.Points} pts";

        public StandingViewModel(IStanding standing)
        {
            _standing = standing;
        }
    }
}
