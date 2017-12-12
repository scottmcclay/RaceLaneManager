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
            Clients.Caller.getTournamentsResponse(TournamentManager.GetTournaments());
        }

        public void RequestAddTournament(string name, int numLanes)
        {
            Debug.WriteLine("Creating tournament \"{0}\" with {1} lanes", name, numLanes);

            TournamentManager.AddTournament(name, numLanes);

            Clients.All.tournamentsUpdated(TournamentManager.GetTournaments());
        }

        public void RequestUpdateTournament(int tournamentID, string newName, int numLanes)
        {
            ITournament tournament = TournamentManager.UpdateTournament(tournamentID, newName, numLanes);

            Clients.All.tournamentUpdated(tournament);
        }

        public void RequestGetCars(int tournamentID)
        {
            Clients.Caller.getCarsResponse(TournamentManager.GetCars(tournamentID));
        }

        public void RequestAddCar(int tournamentID, Car car)
        {
            Debug.WriteLine("Adding car \"{0}\" to tournament {1}", car.Name, tournamentID);

            TournamentManager.AddCar(tournamentID, car);

            Clients.All.carsUpdated(tournamentID, TournamentManager.GetCars(tournamentID));
        }

        public void RequestUpdateCar(int tournamentID, Car car)
        {
            ICar updatedCar = TournamentManager.UpdateCar(tournamentID, car);

            Clients.All.carUpdated(tournamentID, updatedCar);
        }

        public void RequestDeleteCar(int tournamentID, int carID)
        {
            ICar deletedCar = TournamentManager.DeleteCar(tournamentID, carID);

            Clients.All.carsUpdated(tournamentID, TournamentManager.GetCars(tournamentID));
        }
    }
}
