class RlmCarsUpdatedEvent {
    tournamentId: number;
    cars: Array<Car>;
}

class RlmCarUpdatedEvent {
    tournamentId: number;
    car: Car;
}

class RlmRacesUpdatedEvent {
    tournamentId: number;
    racesData: RlmRacesData;
}

class RlmCurrentRaceUpdatedEvent {
    tournamentId: number;
    race: Race;
}

class RlmCarLaneAssignment {
    raceNum: number;
    laneNum: number;

    static fromPayload(payload: any): RlmCarLaneAssignment {
        return {
            raceNum: payload.RaceNum,
            laneNum: payload.LaneNum
        };
    }

    static getCarLaneAssignmentsFromPayload(payload: any): Array<RlmCarLaneAssignment> {
        let result: Array<RlmCarLaneAssignment> = new Array<RlmCarLaneAssignment>();
        
        for (let t of payload) {
            result.push(RlmCarLaneAssignment.fromPayload(t));
        }

        return result;
    }
}

class RlmCarRaces {
    car: Car;
    assignments: Array<RlmCarLaneAssignment>;

    static fromPayload(payload: any): RlmCarRaces {
        return {
            car: Car.fromPayload(payload.Car),
            assignments: RlmCarLaneAssignment.getCarLaneAssignmentsFromPayload(payload.Assignments)
        };
    }

    static getCarRacesFromPayload(payload: any): Array<RlmCarRaces> {
        let carRaces: Array<RlmCarRaces> = new Array<RlmCarRaces>();
        
        for (let t of payload) {
            carRaces.push(RlmCarRaces.fromPayload(t));
        }

        return carRaces;
    }
}

class RlmRacesData {
    numLanes: number;
    numCars: number;
    numRaces: number;
    races: Array<Race>;
    racesByCar: Array<RlmCarRaces>;

    static fromPayload(payload: any): RlmRacesData {
        return {
            numLanes: payload.NumLanes,
            numCars: payload.NumCars,
            numRaces: payload.NumRaces,
            races: Race.getRacesFromPayload(payload.Races),
            racesByCar: RlmCarRaces.getCarRacesFromPayload(payload.RacesByCar)
        };
    }
}

class RlmStandingsUpdatedEvent {
    tournamentId: number;
    standings: Array<Standing>;
}

class RlmNextRacesUpdatedEvent {
    tournamentId: number;
    nextRaces: Array<Race>;
}

class RlmHub extends Publisher {
    private connection: SignalR.Hub.Connection;
    private proxy: SignalR.Hub.Proxy;

    static readonly HUB_READY: string = 'hubReady';
    static readonly GET_TOURNAMENTS_RESPONSE: string = 'getTournamentsResponse';
    static readonly TOURNAMENTS_UPDATED: string = 'tournamentsUpdated';
    static readonly TOURNAMENT_UPDATED: string = 'tournamentUpdated';
    static readonly GET_CARS_RESPONSE: string = 'getCarsResponse';
    static readonly CARS_UPDATED: string = 'carsUpdated';
    static readonly CAR_UPDATED: string = 'carUpdated';
    static readonly GET_CURRENT_RACE_RESPONSE: string = 'getCurrentRaceResponse';
    static readonly CURRENT_RACE_UPDATED: string = 'currentRaceUpdated';
    static readonly GET_RACES_RESPONSE: string = 'getRacesResponse';
    static readonly RACES_UPDATED: string = 'racesUpdated';
    static readonly GET_STANDINGS_RESPONSE: string = 'getStandings';
    static readonly STANDINGS_UPDATED: string = 'standingsUpdated';
    static readonly GET_NEXT_RACES_RESPONSE: string = 'getNextRaces';
    static readonly NEXT_RACES_UPDATED: string = 'nextRacesUpdated';

    initialize(hubConnection: SignalR.Hub.Connection): void {
        this.connection = hubConnection;
        this.proxy = this.connection.createHubProxy("rlmHub");
        this.connection.start()
            .done((function () { 
                console.log("Now connected to test hub!!!: ");
                this.fire(RlmHub.HUB_READY);
            }).bind(this))
            .fail(function () { console.log("Unable to connect to hub"); });

        this.proxy.on('tournamentsUpdated', this.tournamentsUpdated.bind(this));
        this.proxy.on('getTournamentsResponse', this.getTournamentsResponse.bind(this));
        this.proxy.on('tournamentUpdated', this.tournamentUpdated.bind(this));
        this.proxy.on('getCarsResponse', this.getCarsResponse.bind(this));
        this.proxy.on('carsUpdated', this.carsUpdated.bind(this));
        this.proxy.on('carUpdated', this.carUpdated.bind(this));
        this.proxy.on('getCurrentRaceResponse', this.getCurrentRaceResponse.bind(this));
        this.proxy.on('currentRaceUpdated', this.currentRaceUpdated.bind(this));
        this.proxy.on('getRacesResponse', this.getRacesResponse.bind(this));
        this.proxy.on('racesUpdated', this.racesUpdated.bind(this));
        this.proxy.on('getStandingsResponse', this.getStandingsResponse.bind(this));
        this.proxy.on('standingsUpdated', this.standingsUpdated.bind(this));
        this.proxy.on('getNextRacesResponse', this.getNextRacesResponse.bind(this));
        this.proxy.on('nextRacesUpdated', this.nextRacesUpdated.bind(this));
    }

    createTournament(name: string, numLanes: number): void {
        this.proxy.invoke('RequestAddTournament', name, numLanes)
            .fail(function (e) { throw new Error('Create tournament failed: ' + e); });
    }

    tournamentsUpdated(payload: any): void {
        console.log("RlmHub:tournamentsUpdated");
        this.fire(RlmHub.TOURNAMENTS_UPDATED, Tournament.getTournamentsFromPayload(payload));
    }

    requestGetTournaments(): void {
        this.proxy.invoke('RequestGetTournaments')
            .fail(function (e) { throw new Error('Get tournaments failed: ' + e); });
    }

    getTournamentsResponse(payload: any): void {
        console.log("RlmHub:getTournamentsResponse");
        this.fire(RlmHub.GET_TOURNAMENTS_RESPONSE, Tournament.getTournamentsFromPayload(payload));
    }

    requestUpdateTournament(id: number, name: string, numLanes: number): void {
        this.proxy.invoke('RequestUpdateTournament', id, name, numLanes)
            .fail(function (e) { throw new Error('Update tournament failed: ' + e); });
    }

    tournamentUpdated(payload: any): void {
        console.log("RlmHub:tournamentUpdated");
        this.fire(RlmHub.TOURNAMENT_UPDATED, Tournament.fromPayload(payload));
    }

    requestGetCars(tournamentId: number): void {
        this.proxy.invoke('RequestGetCars', tournamentId)
            .fail(function (e) { throw new Error('Get cars failed: ' + e); });
    }

    getCarsResponse(payload: any): void {
        console.log("RlmHub:getCarsResponse");
        this.fire(RlmHub.GET_CARS_RESPONSE, Car.getCarsFromPayload(payload));
    }

    requestAddCar(tournamentId: number, car: Car): void {
        this.proxy.invoke('RequestAddCar', tournamentId, car)
            .fail(function (e) { throw new Error('Add car failed: ' + e); });
    }

    requestUpdateCar(tournamentId: number, car: Car): void {
        this.proxy.invoke('RequestUpdateCar', tournamentId, car)
            .fail(function (e) { throw new Error('Update car failed: ' + e); });
    }

    carsUpdated(tournamentId: number, payload: any): void {
        this.fire(RlmHub.CARS_UPDATED, { 'tournamentId': tournamentId, 'cars': Car.getCarsFromPayload(payload) });
    }

    carUpdated(tournamentId: number, payload: any): void {
        this.fire(RlmHub.CAR_UPDATED, { 'tournamentId': tournamentId, 'car': Car.fromPayload(payload) });
    }

    requestDeleteCar(tournamentId: number, carId: number) {
        this.proxy.invoke('RequestDeleteCar', tournamentId, carId)
            .fail(function (e) { throw new Error('Delete car failed: ' + e); });
    }

    requestGetRaces(tournamentId: number): void {
        this.proxy.invoke('RequestGetRaces', tournamentId)
            .fail(function (e) { throw new Error('Get races failed: ' + e); });
    }

    getRacesResponse(payload: any): void {
        console.log("RlmHub:getRacesResponse");
        this.fire(RlmHub.GET_RACES_RESPONSE, RlmRacesData.fromPayload(payload));
    }

    generateRaces(tournamentId: number): void {
        this.proxy.invoke('RequestGenerateRaces', tournamentId)
            .fail(function (e) { throw new Error('Generate races failed: ' + e); });
    }

    racesUpdated(tournamentId: number, payload: any): void {
        this.fire(RlmHub.RACES_UPDATED, { 'tournamentId': tournamentId, 'racesData': RlmRacesData.fromPayload(payload) });
    }

    requestGetCurrentRace(tournamentId: number): void {
        this.proxy.invoke('RequestGetCurrentRace', tournamentId)
            .fail(function(e) { throw new Error('Get current race failed: ' + e); });
    }

    getCurrentRaceResponse(payload: any): void {
        console.log("RlmHub:getCurrentRaceResponse");
        this.fire(RlmHub.GET_CURRENT_RACE_RESPONSE, Race.fromPayload(payload));
    }

    currentRaceUpdated(tournamentId: number, payload: any): void {
        console.log("RlmHub:currentRaceUpdated");
        this.fire(RlmHub.CURRENT_RACE_UPDATED, {'tournamentId': tournamentId, 'race': Race.fromPayload(payload)});
    }

    requestGetStandings(tournamentId: number): void {
        this.proxy.invoke('RequestGetStandings', tournamentId)
            .fail(function(e) { throw new Error('Get standings failed: ' + e); });
    }

    getStandingsResponse(payload: any): void {
        console.log("RlmHub:getStandingsResponse");
        this.fire(RlmHub.GET_STANDINGS_RESPONSE, Standing.getStandingsFromPayload(payload));
    }

    standingsUpdated(tournamentId: number, payload: any): void {
        console.log("RlmHub:standingsUpdated");
        this.fire(RlmHub.STANDINGS_UPDATED, {'tournamentId': tournamentId, 'standings': Standing.getStandingsFromPayload(payload)});
    }

    requestGetNextRaces(tournamentId: number): void {
        this.proxy.invoke('RequestGetNextRaces', tournamentId)
            .fail(function(e) { throw new Error('Get next races failed: ' + e); });
    }

    getNextRacesResponse(payload: any): void {
        console.log("RlmHub:getNextRacesResponse");
        this.fire(RlmHub.GET_NEXT_RACES_RESPONSE, Race.getRacesFromPayload(payload));
    }

    nextRacesUpdated(tournamentId: number, payload: any) {
        console.log("RlmHub:nextRacesUpdated");
        this.fire(RlmHub.NEXT_RACES_UPDATED, {'tournamentId': tournamentId, 'nextRaces': Race.getRacesFromPayload(payload)});
    }
}