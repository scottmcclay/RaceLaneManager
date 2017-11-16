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
        this.push('standings', {position: '2nd', car: 'Black Knight', name: 'Owen A.', points: 10});
        this.push('standings', {position: '3rd', car: 'Viper', name: 'Sean J.', points: 9});
    }
}

RlmStandingsList.register();