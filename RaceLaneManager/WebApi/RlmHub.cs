using Microsoft.AspNet.SignalR;
using System;
using System.Diagnostics;
using RaceLaneManager.Model;
using RaceLaneManager.Repository;
using System.Collections.Generic;
using RaceLaneManager.DTOs;

namespace RaceLaneManager.WebApi
{
    public class RlmHub: Hub
    {
        public RlmHub()
        {
        }

        public void RequestGetTournaments()
        {
            Debug.WriteLine("Getting tournaments");

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
            Clients.All.standingsUpdated(tournamentID, TournamentManager.GetStandings(tournamentID));
        }

        public void RequestUpdateCar(int tournamentID, Car car)
        {
            ICar updatedCar = TournamentManager.UpdateCar(tournamentID, car);

            Clients.All.carUpdated(tournamentID, updatedCar);
            Clients.All.standingsUpdated(tournamentID, TournamentManager.GetStandings(tournamentID));
        }

        public void RequestDeleteCar(int tournamentID, int carID)
        {
            ICar deletedCar = TournamentManager.DeleteCar(tournamentID, carID);

            Clients.All.carsUpdated(tournamentID, TournamentManager.GetCars(tournamentID));
            Clients.All.standingsUpdated(tournamentID, TournamentManager.GetStandings(tournamentID));
        }

        public void RequestGenerateRaces(int tournamentID)
        {
            RlmGetRacesResponse response = TournamentManager.GenerateRaces(tournamentID);

            Clients.All.racesUpdated(tournamentID, response);
            Clients.All.currentRaceUpdated(tournamentID, TournamentManager.GetCurrentRace(tournamentID));
            Clients.All.nextRacesUpdated(tournamentID, TournamentManager.GetNextRaces(tournamentID));
            Clients.All.standingsUpdated(tournamentID, TournamentManager.GetStandings(tournamentID));
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

            Clients.All.currentRaceUpdated(tournamentID, TournamentManager.GetCurrentRace(tournamentID));
            Clients.All.nextRacesUpdated(tournamentID, TournamentManager.GetNextRaces(tournamentID));
        }

        public void RequestUpdateRace(int tournamentID, Race race)
        {
            TournamentManager.UpdateRace(tournamentID, race);

            Clients.All.racesUpdated(tournamentID, TournamentManager.GetRaces(tournamentID));
            Clients.All.currentRaceUpdated(tournamentID, TournamentManager.GetCurrentRace(tournamentID));
            Clients.All.nextRacesUpdated(tournamentID, TournamentManager.GetNextRaces(tournamentID));
            Clients.All.standingsUpdated(tournamentID, TournamentManager.GetStandings(tournamentID));
        }

        public void RequestStartRace(int tournamentID, int raceNum)
        {
            TournamentManager.StartRace(tournamentID, raceNum);

            Clients.All.racesUpdated(tournamentID, TournamentManager.GetRaces(tournamentID));
            Clients.All.currentRaceUpdated(tournamentID, TournamentManager.GetCurrentRace(tournamentID));
            Clients.All.nextRacesUpdated(tournamentID, TournamentManager.GetNextRaces(tournamentID));

            RaceMonitor.Monitor("COM4", tournamentID, raceNum);
        }

        public void RequestStopRace(int tournamentID, int raceNum)
        {
            RaceMonitor.Stop();

            TournamentManager.StopRace(tournamentID, raceNum);

            Clients.All.racesUpdated(tournamentID, TournamentManager.GetRaces(tournamentID));
            Clients.All.currentRaceUpdated(tournamentID, TournamentManager.GetCurrentRace(tournamentID));
            Clients.All.nextRacesUpdated(tournamentID, TournamentManager.GetNextRaces(tournamentID));
        }

        public void RequestGetTournamentResults(int tournamentID)
        {
            Clients.Caller.requestGetTournamentResultsResponse(TournamentManager.GetTournamentResults(tournamentID));
        }
    }
}
