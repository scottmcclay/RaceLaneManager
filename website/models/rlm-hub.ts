class RlmCarsUpdatedEvent {
    tournamentId: number;
    cars: Array<Car>;
}

class RlmCarUpdatedEvent {
    tournamentId: number;
    car: Car;
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

    requestGetCurrentRace(tournamentId: number) {
        this.proxy.invoke('RequestGetCurrentRace', tournamentId)
            .fail(function(e) { throw new Error('Get current race failed: ' + e); });
    }

    getCurrentRaceResponse(payload: any): void {
        console.log("RlmHub:getCurrentRaceResponse");
        this.fire(RlmHub.GET_CURRENT_RACE_RESPONSE, Race.fromPayload(payload));
    }

    currentRaceUpdated(tournamentId: number, payload: any): void {
        console.log("RlmHub:currentRaceUpdated");
        this.fire(RlmHub.CURRENT_RACE_UPDATED, {'tournamenId': tournamentId, 'race': Race.fromPayload(payload)});
    }
}