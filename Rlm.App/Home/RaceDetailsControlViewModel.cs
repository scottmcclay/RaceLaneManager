using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rlm.Core;

namespace Rlm.App
{
    class RaceDetailsControlViewModel
    {
        private Race _race;

        public string CurrentRace
        {
            get
            {
                string result = "Race ??";
                if (_race != null)
                {
                    result = $"Race {_race.RaceNumber}";
                }

                return result;
            }
        }

        public string RaceState
        {
            get
            {
                string result = "Unknown";
                if (_race != null)
                {
                    switch (_race.State)
                    {
                        case Core.RaceState.NotStarted:
                            result = "Lining Up";
                            break;

                        case Core.RaceState.Racing:
                            result = "Racing";
                            break;

                        case Core.RaceState.Done:
                            result = "Completed";
                            break;
                    }
                }

                return result;
            }
        }

        public RaceDetailsControlViewModel()
        {
            _race = new Race();
            _race.RaceNumber = 5;
            _race.State = Core.RaceState.NotStarted;
        }

        public RaceDetailsControlViewModel(Race race)
        {
            _race = race;
        }
    }
}
