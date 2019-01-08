using Microsoft.AspNet.SignalR;
using System.Diagnostics;
using Rlm.Core;
using System.Collections.Generic;

namespace RaceLaneManager.WebApi
{
    public class RlmHub: Hub
    {
        public RlmHub()
        {
            TournamentManager.CarsUpdated += TournamentManager_CarsUpdated;
            TournamentManager.CarUpdated += TournamentManager_CarUpdated;
            TournamentManager.RaceUpdated += TournamentManager_RaceUpdated;
            TournamentManager.NextRacesUpdated += TournamentManager_NextRacesUpdated;
            TournamentManager.RacesUpdated += TournamentManager_RacesUpdated;
            TournamentManager.StandingsUpdated += TournamentManager_StandingsUpdated;
            TournamentManager.TournamentsUpdated += TournamentManager_TournamentsUpdated;
            TournamentManager.TournamentUpdated += TournamentManager_TournamentUpdated;
        }

        private void TournamentManager_CarsUpdated(int tournamentID, CarsUpdatedEventArgs e)
        {
            Clients.All.carsUpdated(tournamentID, e.Cars);
        }

        private void TournamentManager_CarUpdated(int tournamentID, CarUpdatedEventArgs e)
        {
            Clients.All.carUpdated(tournamentID, e.Car);
        }

        private void TournamentManager_RaceUpdated(int tournamentID, RaceUpdatedEventArgs e)
        {
            Clients.All.currentRaceUpdated(tournamentID, e.Race);
        }

        private void TournamentManager_NextRacesUpdated(int tournamentID, NextRacesUpdatedEventArgs e)
        {
            Clients.All.nextRacesUpdated(tournamentID, e.NextRaces);
        }

        private void TournamentManager_RacesUpdated(int tournamentID, RacesUpdatedEventArgs e)
        {
            Clients.All.racesUpdated(tournamentID, e.Races);
        }

        private void TournamentManager_StandingsUpdated(int tournamentID, StandingsUpdatedEventArgs e)
        {
            Clients.All.standingsUpdated(tournamentID, e.Standings);
        }

        private void TournamentManager_TournamentsUpdated(TournamentsUpdatedEventArgs e)
        {
            Clients.All.tournamentsUpdated(e.Tournaments);
        }

        private void TournamentManager_TournamentUpdated(TournamentUpdatedEventArgs e)
        {
            Clients.All.tournamentUpdated(e.Tournament);
        }

        public void RequestGetTournaments()
        {
            Clients.Caller.getTournamentsResponse(TournamentManager.GetTournaments());
        }

        public void RequestAddTournament(string name, int numLanes)
        {
            Debug.WriteLine("Creating tournament \"{0}\" with {1} lanes", name, numLanes);

            TournamentManager.AddTournament(name, numLanes);
        }

        public void RequestUpdateTournament(int tournamentID, string newName, int numLanes)
        {
            TournamentManager.UpdateTournament(tournamentID, newName, numLanes);
        }

        public void RequestGetCars(int tournamentID)
        {
            Clients.Caller.getCarsResponse(TournamentManager.GetCars(tournamentID));
        }

        public void RequestAddCar(int tournamentID, Car car)
        {
            Debug.WriteLine("Adding car \"{0}\" to tournament {1}", car.Name, tournamentID);

            TournamentManager.AddCar(tournamentID, car);
        }

        public void RequestUpdateCar(int tournamentID, Car car)
        {
            TournamentManager.UpdateCar(tournamentID, car);
        }

        public void RequestDeleteCar(int tournamentID, int carID)
        {
            TournamentManager.DeleteCar(tournamentID, carID);
        }

        public void RequestGenerateRaces(int tournamentID)
        {
            TournamentManager.GenerateRaces(tournamentID);
        }

        public void RequestGetRaces(int tournamentID)
        {
            Clients.Caller.getRacesResponse(TournamentManager.GetRaces(tournamentID));
        }

        public void RequestGetCurrentRace(int tournamentID)
        {
            Clients.Caller.getCurrentRaceResponse(TournamentManager.GetCurrentRace(tournamentID));
        }

        public void RequestGetStandings(int tournamentID)
        {
            Clients.Caller.getStandingsResponse(TournamentManager.GetStandings(tournamentID));
        }

        public void RequestGetNextRaces(int tournamentID)
        {
            Clients.Caller.getNextRacesResponse(TournamentManager.GetNextRaces(tournamentID));
        }

        public void RequestSetCurrentRace(int tournamentID, int raceNum)
        {
            TournamentManager.SetCurrentRace(tournamentID, raceNum);
        }

        public void RequestUpdateRace(int tournamentID, Race race)
        {
            TournamentManager.UpdateRace(tournamentID, race);
        }

        public void RequestStartRace(int tournamentID, int raceNum)
        {
            TournamentManager.StartRace(tournamentID, raceNum);
        }

        public void RequestStopRace(int tournamentID, int raceNum)
        {
            TournamentManager.StopRace(tournamentID, raceNum);
        }

        public void RequestGetTournamentResults(int tournamentID)
        {
            Clients.Caller.requestGetTournamentResultsResponse(TournamentManager.GetTournamentResults(tournamentID));
        }
    }
}
