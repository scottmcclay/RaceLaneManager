using RaceLaneManager.Model;
using System.Collections.Generic;

namespace RaceLaneManager.Repository
{
    public interface IRlmRepository
    {
        IEnumerable<int> GetAllTournamentIDs();
        Tournament LoadTournament(int tournamentID);
        void SaveTournament(Tournament tournament);
    }
}
