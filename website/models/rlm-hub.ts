class RlmHub extends Publisher {
    private connection: SignalR.Hub.Connection;
    private proxy: SignalR.Hub.Proxy;

    initialize(hubConnection: SignalR.Hub.Connection): void {
        this.connection = hubConnection;
        this.proxy = this.connection.createHubProxy("rlmHub");
        this.connection.start()
            .done((function () { 
                console.log("Now connected to test hub!!!: ");
                this.fire('hubReady');
            }).bind(this))
            .fail(function () { console.log("Unable to connect to hub"); });

        this.proxy.on('tournamentsUpdated', this.tournamentsUpdated.bind(this));
        this.proxy.on('getTournamentsResponse', this.getTournamentsResponse.bind(this));
        this.proxy.on('tournamentUpdated', this.tournamentUpdated.bind(this));
    }

    createTournament(name: string, numLanes: number): void {
        this.proxy.invoke('RequestAddTournament', name, numLanes);
    }

    tournamentsUpdated(payload: any): void {
        console.log("RlmHub:tournamentsUpdated");
        this.fire('tournamentsUpdated', this.getTournamentsFromPayload(payload));
    }

    requestGetTournaments(): void {
        this.proxy.invoke('RequestGetTournaments');
    }

    getTournamentsResponse(payload: any): void {
        console.log("RlmHub:getTournamentsResponse");
        this.fire('getTournamentsResponse', this.getTournamentsFromPayload(payload));
    }

    requestUpdateTournament(id: number, name: string, numLanes: number): void {
        this.proxy.invoke('RequestUpdateTournament', id, name, numLanes);
    }

    tournamentUpdated(payload: any): void {
        console.log("RlmHub:tournamentUpdated");
        this.fire('tournamentUpdated', Tournament.fromPayload(payload));
    }

    private getTournamentsFromPayload(payload: any): Array<Tournament> {
        let tournaments: Array<Tournament> = new Array<Tournament>();

        for (let t of payload) {
            tournaments.push(Tournament.fromPayload(t));
        }

        return tournaments;
    }
}