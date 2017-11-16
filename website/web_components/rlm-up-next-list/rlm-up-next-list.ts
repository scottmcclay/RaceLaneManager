@component("rlm-up-next-list")
class RlmUpNextList extends polymer.Base implements polymer.Element {

    @property({type: Tournament, reflectToAttribute: true, notify: true})
    tournament: Tournament;

    @property({type: [NextRace], notify: true})
    nextRaces: Array<NextRace>;

    constructor() {
        super();
        this.nextRaces = new Array<NextRace>();
        this.populateDummyData();
    }

    populateDummyData() {
        let nextRace: NextRace = new NextRace();
        nextRace.raceNum = 5;
        nextRace.lanes.push({lane: 1, car: 'Viper', racer: 'Sean J.'});
        nextRace.lanes.push({lane: 2, car: 'Black Night', racer: 'Owen A.'});
        nextRace.lanes.push({lane: 3, car: 'Lightning', racer: 'Noob'});
        nextRace.lanes.push({lane: 4, car: 'Ghost', racer: 'Brenden M.'});
        this.push('nextRaces', nextRace);
    }
}

RlmUpNextList.register();