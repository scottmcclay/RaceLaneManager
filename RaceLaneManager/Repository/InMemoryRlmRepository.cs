using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaceLaneManager.Model;

namespace RaceLaneManager.Repository
{
    public class InMemoryRlmRepository : IRlmRepository
    {
        private static List<Tournament> _tournaments = new List<Tournament>();

        public IEnumerable<int> GetAllTournamentIDs()
        {
            return _tournaments.Select(t => t.ID).ToList();
        }

        public Tournament LoadTournament(int tournamentID)
        {
            return _tournaments.Where(t => t.ID == tournamentID).Single();
        }

        public void SaveTournament(Tournament tournament)
        {
            for (int i = 0; i < _tournaments.Count; i++)
            {
                if (_tournaments[i].ID == tournament.ID)
                {
                    _tournaments[i] = tournament;
                }
            }
        }
    }
}
