@component('rlm-standings-list')
class RlmStandingsList extends polymer.Base implements polymer.Element {

    @property({type: Tournament, reflectToAttribute: true, notify: true})
    tournament: Tournament;

    @property({type: Array})
    standings: Array<Standing>;

    constructor() {
        super();

        this.standings = new Array<Standing>();
        this.populateSampleData();
    }

    populateSampleData() {
        this.push('standings', {position: '1st', car: 'Ghost', name: 'Brenden M.', points: 13});
        this.push('standings', {position: '2nd', car: 'Black Knight of the desert', name: 'Owen A.', points: 10});
        this.push('standings', {position: '3rd', car: 'Viper', name: 'Sean J.', points: 9});
        this.push('standings', {position: '4th', car: 'Phantom', name: 'Sean J.', points: 7});
        this.push('standings', {position: '5th', car: 'Vector', name: 'Sean J.', points: 6});
        this.push('standings', {position: '6th', car: 'Lightning', name: 'Sean J.', points: 4});
        this.push('standings', {position: '7th', car: 'Cosmo', name: 'Sean J.', points: 2});
        this.push('standings', {position: '8th', car: 'Aurora', name: 'Sean J.', points: 1});
    }
}

RlmStandingsList.register();