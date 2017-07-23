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

        public IList<Tournament> GetAllTournaments()
        {
            return _tournaments;
        }

        public Tournament GetTournament(int tournamentId)
        {
            return _tournaments.Where(t => t.ID == tournamentId).Single();
        }

        public Tournament AddTournament(Tournament tournament)
        {
            // find an ID to assign to this new Tournament
            int maxID = 0;
            foreach (Tournament t in _tournaments)
            {
                if (t.ID > maxID)
                {
                    maxID = t.ID;
                }
            }

            tournament.ID = maxID + 1;
            _tournaments.Add(tournament);

            return tournament;
        }

        public bool UpdateTournament(Tournament tournament)
        {
            return true;
        }

        public Tournament DeleteTournament(int tournamentId)
        {
            Tournament tournament = _tournaments.Where(t => t.ID == tournamentId).Single();
            if (tournament == null)
            {
                throw new ArgumentException(string.Format("Tournament with ID {0} was not found.", tournamentId), "tournamentId");
            }

            _tournaments.Remove(tournament);
            return tournament;
        }
    }
}
