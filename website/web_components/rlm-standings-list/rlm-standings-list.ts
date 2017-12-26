@component('rlm-standings-list')
class RlmStandingsList extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true, reflectToAttribute: true})
    hub: RlmHub;

    @property({ type: Number, reflectToAttribute: true, notify: true })
    tournamentId: number;

    @property({ type: [Standing], notify: true })
    standings: Array<Standing>;

    @observe('hub')
    hubChanged(): void {
        console.log("rlm-standings-list.hubChanged");
        if (this.hub) {
            this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
            this.hub.on(RlmHub.GET_STANDINGS_RESPONSE, this.getStandingsResponse, this);
            this.hub.on(RlmHub.STANDINGS_UPDATED, this.standingsUpdated, this);
        }

        this.getStandings();
    }

    hubReady(): void {
        console.log("rlm-standings-list.hubReady");
        this.getStandings();
    }

    @observe('tournamentId')
    tournamentIdChanged(): void {
        console.log("rlm-standings-list.tournamentIdChanged");
        this.getStandings();
    }

    private getStandings(): void {
        console.log("rlm-standings-list.getStandings");
        this.standings = [];

        if (this.tournamentId && this.hub) {
            console.log("rlm-standings-list.getStandings: calling requestGetStandings");
            this.hub.requestGetStandings(this.tournamentId);
        }
    }

    getStandingsResponse(standings: Array<Standing>): void {
        this.set('standings', standings);
    }

    standingsUpdated(e: RlmStandingsUpdatedEvent): void {
        if (e.tournamentId === this.tournamentId) {
            this.set('standings', e.standings);
        }
    }
}

RlmStandingsList.register();