@component('rlm-races')
class RlmRaces extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true, reflectToAttribute: true})
    hub: RlmHub;

    @property({ type: Number, reflectToAttribute: true, notify: true })
    tournamentId: number;

    @property({ type: [String], notify: true})
    laneNames: Array<string>;

    @property({ type: [Race], notify: true })
    races: Array<Race>;

    @property({ type: [RlmCarRaces], notify: true })
    racesByCar: Array<RlmCarRaces>;

    ready(): void {
        console.log("rlm-races.ready");
    }

    @observe('hub')
    hubChanged(): void {
        console.log("rlm-races.hubChanged");
        if (this.hub) {
            this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
            this.hub.on(RlmHub.GET_RACES_RESPONSE, this.getRacesResponse, this);
            this.hub.on(RlmHub.RACES_UPDATED, this.racesUpdated, this);
        }

        this.getRaces();
    }

    hubReady(): void {
        console.log("rlm-races.hubReady");
        this.getRaces();
    }

    @observe('tournamentId')
    tournamentIdChanged(): void {
        console.log("rlm-races.tournamentIdChagned");
        this.getRaces();
    }

    private getRaces(): void {
        console.log("rlm-races.getRaces");
        this.races = [];

        if (this.tournamentId && this.hub) {
            console.log("rlm-races.getRaces: calling requestGetRaces");
            this.hub.requestGetRaces(this.tournamentId);
        }
    }

    getRacesResponse(racesData: RlmRacesData): void {
        this.showRaces(racesData);
    }

    racesUpdated(e: RlmRacesUpdatedEvent): void {
        if (e.tournamentId === this.tournamentId) {
            this.showRaces(e.racesData);
        }
    }

    generateRaces(): void {
        this.hub.generateRaces(this.tournamentId);
    }

    showRaces(racesData: RlmRacesData): void {
        this.set('races', racesData.races);
        this.set('racesByCar', racesData.racesByCar);
        this.laneNames = [];

        for (let i = 1; i <= racesData.numLanes; i++) {
            this.push('laneNames', 'Lane ' + i);
        }
    }
}

RlmRaces.register();
