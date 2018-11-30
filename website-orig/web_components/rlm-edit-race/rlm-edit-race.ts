@component('rlm-edit-race')
class RlmEditRace extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true, reflectToAttribute: true})
    hub: RlmHub;

    @property({ type: Number, reflectToAttribute: true, notify: true })
    tournamentId: number;

    @property({ type: [Race], notify: true })
    races: Array<Race>;

    @property({ type: Number, notify: true, value: 3 })
    currentRace: number;

    @observe('hub')
    hubChanged(): void {
        console.log("rlm-edit-race.hubChanged");
        if (this.hub) {
            this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
            this.hub.on(RlmHub.GET_RACES_RESPONSE, this.getRacesResponse, this);
            this.hub.on(RlmHub.RACES_UPDATED, this.racesUpdated, this);
            this.hub.on(RlmHub.GET_CURRENT_RACE_RESPONSE, this.getCurrentRaceResponse, this);
            this.hub.on(RlmHub.CURRENT_RACE_UPDATED, this.currentRaceUpdated, this);
        }

        this.getRaces();
    }

    hubReady(): void {
        console.log("rlm-edit-race.hubReady");
        this.getRaces();
    }

    @observe('tournamentId')
    tournamentIdChanged(): void {
        console.log("rlm-edit-race.tournamentIdChagned");
        this.getRaces();
    }

    private getRaces(): void {
        console.log("rlm-edit-race.getRaces");
        this.races = [];

        if (this.tournamentId && this.hub) {
            console.log("rlm-edit-race.getRaces: calling requestGetRaces");
            this.hub.requestGetRaces(this.tournamentId);
        }
    }

    hideCurrentRaceDecorator(raceNum: number, currentRace: number): boolean {
        return !(raceNum === currentRace);
    }

    saveRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('saveRace ' + raceNum);
        let race: Race = undefined;
        for (let r of this.races) {
            if (r.raceNum === raceNum) {
                race = r;
                break;
            }
        }

        if (race) {
            console.log('Updating race ' + raceNum);
            this.hub.requestUpdateRace(this.tournamentId, race);
        }
        else {
            console.error('Could not find race ' + raceNum);
        }
    }

    revertRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('revertRace ' + raceNum);
        this.hub.requestGetRaces(this.tournamentId);
    }

    makeCurrentRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('makeCurrentRace ' + raceNum);
        this.hub.setCurrentRace(this.tournamentId, raceNum);
    }

    startRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('startRace ' + raceNum);
        this.hub.requestStartRace(this.tournamentId, raceNum);
    }

    stopRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('stopRace ' + raceNum);
        this.hub.requestStopRace(this.tournamentId, raceNum);
    }

    getRacesResponse(racesData: RlmRacesData): void {
        this.showRaces(racesData);
    }

    racesUpdated(e: RlmRacesUpdatedEvent): void {
        if (e.tournamentId === this.tournamentId) {
            this.showRaces(e.racesData);
        }
    }

    getCurrentRaceResponse(race: Race): void {
        this.currentRace = race.raceNum;
    }

    currentRaceUpdated(e: RlmCurrentRaceUpdatedEvent): void {
        if (e.tournamentId === this.tournamentId) {
            this.currentRace = e.race.raceNum;

            let index = 0;
            while (index < this.races.length) {
                if (this.races[index].raceNum === this.currentRace) {
                    break;
                }
                index++;
            }

            if (index < this.races.length) {
                this.splice('races', index, 1, e.race);
            }
        }
    }

    showRaces(racesData: RlmRacesData): void {
        this.set('races', racesData.races);
    }
}

RlmEditRace.register();
