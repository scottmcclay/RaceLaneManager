@component("rlm-up-next-list")
class RlmUpNextList extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true, reflectToAttribute: true})
    hub: RlmHub;

    @property({ type: Number, reflectToAttribute: true, notify: true })
    tournamentId: number;

    @property({type: [Race], notify: true})
    nextRaces: Array<Race>;

    @observe('hub')
    hubChanged(): void {
        console.log("rlm-up-next-list.hubChanged");
        if (this.hub) {
            this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
            this.hub.on(RlmHub.GET_NEXT_RACES_RESPONSE, this.getNextRacesResponse, this);
            this.hub.on(RlmHub.NEXT_RACES_UPDATED, this.nextRacesUpdated, this);
        }

        this.getNextRaces();
    }

    hubReady(): void {
        console.log("rlm-up-next-list.hubReady");
        this.getNextRaces();
    }

    @observe('tournamentId')
    tournamentIdChanged(): void {
        console.log("rlm-up-next-list.tournamentIdChanged");
        this.getNextRaces();
    }

    private getNextRaces(): void {
        console.log("rlm-up-next-list.getNextRaces");
        this.nextRaces = [];

        if (this.tournamentId && this.hub) {
            console.log("rlm-up-next-list.getNextRaces: calling requestGetNextRaces");
            this.hub.requestGetNextRaces(this.tournamentId);
        }
    }

    getNextRacesResponse(nextRaces: Array<Race>): void {
        this.set('nextRaces', nextRaces);
    }

    nextRacesUpdated(e: RlmNextRacesUpdatedEvent): void {
        if (e.tournamentId === this.tournamentId) {
            this.set('nextRaces', e.nextRaces);
        }
    }
}

RlmUpNextList.register();