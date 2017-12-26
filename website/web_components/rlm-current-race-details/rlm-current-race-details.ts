@component("rlm-current-race-details")
class RlmCurrentRaceDetails extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true, reflectToAttribute: true})
    hub: RlmHub;

    @property({ type: Number, reflectToAttribute: true, notify: true })
    tournamentId: number;

    @property({ type: Race })
    currentRace: Race;

    private getDummyData() {
        // this.lanes = new Array<LaneAssignment>();
        // this.push('lanes', {lane: 1, car: {id: 1, name: 'Ghost', owner: 'Brenden M.', den: 'Bear', carNumber: 1}, elapsedTime: 0});
        // this.push('lanes', {lane: 2, car: {id: 2, name: 'Black Night of the desert with a whole lot of extra text in order to see how long strings are handled', owner: 'Owen A.', den: 'Bear', carNumber: 2}, elapsedTime: 0});
        // this.push('lanes', {lane: 3, car: {id: 3, name: 'Viper', owner: 'Sean J.', den: 'Bear', carNumber: 3}, elapsedTime: 0});
        // this.push('lanes', {lane: 4, car: {id: 4, name: 'Lightning', owner: 'Noob', den: 'Bear', carNumber: 4}, elapsedTime: 0});
        // this.push('lanes', {lane: 5, car: {id: 5, name: 'Phantom', owner: 'Luca K.', den: 'Wolf', carNumber: 5}, elapsedTime: 0});
        // this.push('lanes', {lane: 6, car: {id: 6, name: 'Vector', owner: 'Bryce H.', den: 'Tiger', carNumber: 6}, elapsedTime: 0});
    }

    @observe('hub')
    hubChanged(): void {
        console.log("rlm-current-race-details.hubChanged");
        if (this.hub) {
            this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
            this.hub.on(RlmHub.GET_CURRENT_RACE_RESPONSE, this.getCurrentRaceResponse, this);
            this.hub.on(RlmHub.CURRENT_RACE_UPDATED, this.currentRaceUpdated, this);
        }

        this.getCurrentRace();
    }

    hubReady(): void {
        console.log("rlm-current-race-details.hubReady");
        this.getCurrentRace();
    }

    @observe('tournamentId')
    tournamentIdChanged(): void {
        console.log("rlm-current-race-details.tournamentIdChagned");
        this.getCurrentRace();
    }

    private getCurrentRace(): void {
        console.log("rlm-current-race-details.getCurrentRace");
        this.currentRace = undefined;

        if (this.tournamentId && this.hub) {
            console.log("rlm-current-race-details.getCurrentRace: calling requestGetCurrentRace");
            this.hub.requestGetCurrentRace(this.tournamentId);
        }
    }

    getCurrentRaceResponse(currentRace: Race): void {
        this.set('currentRace', currentRace);
    }

    currentRaceUpdated(e: RlmCurrentRaceUpdatedEvent): void {
        if (e.tournamentId === this.tournamentId) {
            this.set('currentRace', e.race);
        }
    }
}

RlmCurrentRaceDetails.register();