using Microsoft.AspNet.SignalR;
using System;
using System.Diagnostics;
using RaceLaneManager.Model;
using RaceLaneManager.Repository;

namespace RaceLaneManager.WebApi
{
    public class RlmHub: Hub
    {
        public RlmHub()
        {
        }

        public void RequestGetTournaments()
        {
            Clients.Caller.getTournamentsResponse(RepositoryManager.GetDefaultRepository().GetAllTournaments());
        }

        public void RequestAddTournament(string name, int numLanes)
        {
            Debug.WriteLine("Creating tournament \"{0}\" with {1} lanes", name, numLanes);

            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            Tournament t = new Tournament(name, numLanes);
            repo.AddTournament(t);

            Clients.All.tournamentsUpdated(repo.GetAllTournaments());
        }

        public void RequestUpdateTournament(int tournamentID, string newName, int numLanes)
        {
            IRlmRepository repo = RepositoryManager.GetDefaultRepository();

            Tournament t = repo.UpdateTournament(tournamentID, newName, numLanes);

            Clients.All.tournamentUpdated(t);
        }
    }
}
