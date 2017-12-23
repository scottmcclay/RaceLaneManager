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
        nextRace.raceNum = 6;
        nextRace.lanes.push({lane: 1, car: 'Viper', racer: 'Sean J.'});
        nextRace.lanes.push({lane: 2, car: 'Black Night', racer: 'Owen A.'});
        nextRace.lanes.push({lane: 3, car: 'Lightning', racer: 'Noob'});
        nextRace.lanes.push({lane: 4, car: 'Ghost', racer: 'Brenden M.'});
        nextRace.lanes.push({lane: 5, car: 'Vector', racer: 'Bryce H.'});
        nextRace.lanes.push({lane: 6, car: 'Phantom', racer: 'Someone with a really really really long name'});
        this.push('nextRaces', nextRace);

        let anotherRace: NextRace = new NextRace();
        anotherRace.raceNum = 7;
        anotherRace.lanes.push({lane: 1, car: 'Phantom', racer: 'Someone with a really really really long name'});
        anotherRace.lanes.push({lane: 2, car: 'Vector', racer: 'Bryce H.'});
        anotherRace.lanes.push({lane: 3, car: 'Viper', racer: 'Sean J.'});
        anotherRace.lanes.push({lane: 4, car: 'Lightning', racer: 'Noob'});
        anotherRace.lanes.push({lane: 5, car: 'Black Night', racer: 'Owen A.'});
        anotherRace.lanes.push({lane: 6, car: 'Ghost', racer: 'Brenden M.'});
        this.push('nextRaces', anotherRace);
    }
}

RlmUpNextList.register();