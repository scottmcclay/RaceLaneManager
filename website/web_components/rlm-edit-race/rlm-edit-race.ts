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

    hideCurrentRaceDecorator(raceNum: number): boolean {
        return !(raceNum === this.currentRace);
    }

    saveRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('saveRace ' + raceNum);

    }

    revertRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('revertRace ' + raceNum);

    }

    makeCurrentRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('makeCurrentRace ' + raceNum);
    }

    getRacesResponse(racesData: RlmRacesData): void {
        this.showRaces(racesData);
    }

    racesUpdated(e: RlmRacesUpdatedEvent): void {
        if (e.tournamentId === this.tournamentId) {
            this.showRaces(e.racesData);
        }
    }

    showRaces(racesData: RlmRacesData): void {
        this.set('races', racesData.races);
    }

    editRace(e: any): void {
        let raceNum: number = e.target.raceNum;
        console.log('editRace ' + raceNum);

        let race: Race = undefined;
        this.races.forEach(r => { if (r.raceNum === raceNum) race = r; });

        if (race !== undefined) {
            let editDialog: RlmEditRaceDialog = this.$.rlmEditRaceDialog as RlmEditRaceDialog;
            editDialog.title = "Edit Race";
            editDialog.race = race;
            editDialog.open();
        }
        else {
            console.error("Unable to find race number " + raceNum);
        }
    }
}

RlmEditRace.register();
