class RlmHub extends Publisher {
    private connection: SignalR.Hub.Connection;
    private proxy: SignalR.Hub.Proxy;

    initialize(hubConnection: SignalR.Hub.Connection): void {
        this.connection = hubConnection;
        this.proxy = this.connection.createHubProxy("rlmHub");
        this.connection.start()
            .done((function () { 
                console.log("Now connected to test hub!!!: ");
                this.fire('proxyReady');
            }).bind(this))
            .fail(function () { console.log("Unable to connect to hub"); });

        this.proxy.on('tournamentsUpdated', (t: any) => { this.tournamentsUpdated(this, t) });
        this.proxy.on('getTournamentsResponse', (t: any) => { this.getTournamentsResponse(this, t) });
    }

    createTournament(name: string, numLanes: number): void {
        this.proxy.invoke('RequestAddTournament', name, numLanes);
    }

    tournamentsUpdated(hub: RlmHub, payload: any): void {
        console.log("RlmHub:tournamentsUpdated");
        this.fire('tournamentsUpdated', this.getTournamentsFromPayload(payload));
    }

    requestGetTournaments(): void {
        this.proxy.invoke('RequestGetTournaments');
    }

    getTournamentsResponse(hub: RlmHub, payload: any): void {
        console.log("RlmHub:getTournamentsResponse");
        this.fire('getTournamentsResponse', this.getTournamentsFromPayload(payload));
    }

    private getTournamentsFromPayload(payload: any): Array<Tournament> {
        let tournaments: Array<Tournament> = new Array<Tournament>();

        for (let t of payload) {
            tournaments.push(this.getTournamentFromPayload(t));
        }

        return tournaments;
    }

    private getTournamentFromPayload(payload: any): Tournament {
        let tournament: Tournament = new Tournament();

        tournament.id = payload.ID;
        tournament.name = payload.Name;
        tournament.numLanes = payload.NumLanes;
        tournament.numRaces = payload.numRaces;

        return tournament;
    }
}