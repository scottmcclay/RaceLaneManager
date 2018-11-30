@component('rlm-home')
class RlmHome extends polymer.Base implements polymer.Element {

    // private connection: SignalR.Hub.Connection;
    @property({ type: RlmHub, notify: true, reflectToAttribute: true})
    hub: RlmHub;

    @property({ type: Number, reflectToAttribute: true, notify: true })
    tournamentId: number;

    @property({ type: Race, reflectToAttribute: true, notify: true })
    currentRace: Race;

    ready(): void {
        console.log("rlm-home.ready");
    }

    @observe('hub')
    hubChanged(): void {
        console.log("rlm-home.hubChanged");
        if (this.hub) {
            this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
            this.hub.on(RlmHub.GET_CURRENT_RACE_RESPONSE, this.getCurrentRaceResponse, this);
        }
    }

    hubReady(): void {
        console.log("rlm-home.hubReady");
    }

    @observe('tournamentId')
    tournamentIdChanged(): void {
        console.log("rlm-home.tournamentIdChagned");
        this.getCurrentRace();
    }

    private getCurrentRace(): void {
        console.log("rlm-home.getCurrentRace");

        if (this.tournamentId && this.hub) {
            console.log("rlm-home.getCurrentRace: calling requestGetCurrentRace");
            this.hub.requestGetCurrentRace(this.tournamentId);
        }
    }

    getCurrentRaceResponse(race: Race): void {
        this.set('currentRace', race);
    }
}

RlmHome.register();