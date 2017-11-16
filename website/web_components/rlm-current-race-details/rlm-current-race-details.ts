@component("rlm-current-race-details")
class RlmCurrentRaceDetails extends polymer.Base implements polymer.Element {

    @property({ type: Tournament, notify: true, reflectToAttribute: true })
    tournament: Tournament;

    @property({ type: [RaceLaneAssignment] })
    lanes: Array<RaceLaneAssignment>;

    constructor() {
        super();

        this.getDummyData();
    }

    private getDummyData() {
        this.lanes = new Array<RaceLaneAssignment>();
        this.push('lanes', {raceNum: 1, lane: {laneNum: 1, active: true}, car: {id: 1, name: 'Ghost', owner: 'Brenden M.', den: 'Bear', carNumber: 1}, elapsedTime: 0});
        this.push('lanes', {raceNum: 1, lane: {laneNum: 2, active: true}, car: {id: 2, name: 'Black Night of the desert', owner: 'Owen A.', den: 'Bear', carNumber: 2}, elapsedTime: 0});
        this.push('lanes', {raceNum: 1, lane: {laneNum: 3, active: true}, car: {id: 3, name: 'Viper', owner: 'Sean J.', den: 'Bear', carNumber: 3}, elapsedTime: 0});
        this.push('lanes', {raceNum: 1, lane: {laneNum: 4, active: true}, car: {id: 4, name: 'Lightning', owner: 'Noob', den: 'Bear', carNumber: 4}, elapsedTime: 0});
        this.push('lanes', {raceNum: 1, lane: {laneNum: 5, active: true}, car: {id: 5, name: 'Phantom', owner: 'Luca K.', den: 'Wolf', carNumber: 5}, elapsedTime: 0});
        this.push('lanes', {raceNum: 1, lane: {laneNum: 6, active: true}, car: {id: 6, name: 'Vector', owner: 'Bryce H.', den: 'Tiger', carNumber: 6}, elapsedTime: 0});
    }
}

RlmCurrentRaceDetails.register();