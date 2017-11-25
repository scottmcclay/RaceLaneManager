using RaceLaneManager.Model;
using System.Collections.Generic;

namespace RaceLaneManager.Repository
{
    public interface IRlmRepository
    {
        IList<Tournament> GetAllTournaments();
        Tournament GetTournament(int tournamentId);
        Tournament AddTournament(Tournament tournament);
        Tournament DeleteTournament(int tournamentId);
        Tournament UpdateTournament(int tournamentId, string newName, int numLanes);
    }
}
