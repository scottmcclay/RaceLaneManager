@component('rlm-results')
class RlmResults extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true, reflectToAttribute: true})
    hub: RlmHub;

    @property({ type: Number, reflectToAttribute: true, notify: true })
    tournamentId: number;

    @property({ type: [Standing], notify: true })
    overallStandings: Array<Standing>;

    @property({ type: [Standing], notify: true })
    tigerStandings: Array<Standing>;

    @property({ type: [Standing], notify: true })
    wolfStandings: Array<Standing>;

    @property({ type: [Standing], notify: true })
    bearStandings: Array<Standing>;

    @property({ type: [Standing], notify: true })
    webelosIStandings: Array<Standing>;

    @property({ type: [Standing], notify: true })
    webelosIIStandings: Array<Standing>;

    

    @observe('hub')
    hubChanged(): void {
        console.log("rlm-results.hubChanged");
        if (this.hub) {
            this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
            this.hub.on(RlmHub.GET_TOURNAMENT_RESULTS_RESPONSE, this.getTournamentResultsResponse, this);
        }

        this.getTournamentResults();
    }

    hubReady(): void {
        console.log("rlm-results.hubReady");
        this.getTournamentResults();
    }

    @observe('tournamentId')
    tournamentIdChanged(): void {
        console.log("rlm-results.tournamentIdChagned");
        this.getTournamentResults();
    }

    private getTournamentResults(): void {
        console.log("rlm-results.getTournamentResults");
        this.overallStandings = [];
        this.tigerStandings = [];
        this.wolfStandings = [];
        this.bearStandings = [];
        this.webelosIStandings = [];
        this.webelosIIStandings = [];

        if (this.tournamentId && this.hub) {
            console.log("rlm-results.getTournamentResults: calling requestGetTournamentResults");
            this.hub.requestGetTournamentResults(this.tournamentId);
        }
    }

    private getTournamentResultsResponse(results: Array<GroupResult>): void {
        for (let gr of results) {
            switch (gr.groupName) {
                case "Overall":
                    this.set('overallStandings', gr.standings);
                    break;

                case "Tiger":
                    this.set('tigerStandings', gr.standings);
                    break;

                case "Wolf":
                    this.set('wolfStandings', gr.standings);
                    break;

                case "Bear":
                    this.set('bearStandings', gr.standings);
                    break;

                case "Webelos I":
                    this.set('webelosIStandings', gr.standings);
                    break;

                case "Webelos II":
                    this.set('webelosIIStandings', gr.standings);
                    break;
            }
        }
    }
}

RlmResults.register();
