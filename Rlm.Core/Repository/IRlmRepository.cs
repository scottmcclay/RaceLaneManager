using System.Collections.Generic;

namespace Rlm.Core
{
    public interface IRlmRepository
    {
        IEnumerable<int> GetAllTournamentIDs();
        Tournament LoadTournament(int tournamentID);
        void SaveTournament(Tournament tournament);
    }
}
