@component('rlm-settings')
class RlmSettings extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true, reflectToAttribute: true })
    hub: RlmHub;
    
    @property({ type: Tournament, notify: true, reflectToAttribute: true })
    tournament: Tournament;

    @property({ type: String, notify: true })
    tournamentName: string;

    @property({ type: Number, notify: true })
    numLanes: number;

    @observe('hub')
    hubChanged(): void {
        if (this.hub) {
            this.hub.on('tournamentUpdated', this.tournamentUpdated, this);
        }
    }

    ready(): void {
        let settings = this.$.tournamentSettings;
        settings.addEventListener(RlmTournamentSettings.SAVE_TAPPED, this.saveSettings.bind(this));
        settings.addEventListener(RlmTournamentSettings.CANCEL_TAPPED, this.cancelEditSettings.bind(this));
    }

    tournamentUpdated(tournament: Tournament): void {
        if (tournament.id === this.tournament.id) {
            this.tournament = tournament;
        }
    }

    @observe('tournament')
    tournamentChanged(newValue: Tournament, oldValue: Tournament): void {
        if (this.tournament) {
            this.tournamentName = this.tournament.name;
            this.numLanes = this.tournament.numLanes;
        }
        else {
            this.tournamentName = "Unknown";
            this.numLanes = 4;
        }
    }

    editSettings(): void {
        let settings = this.$.tournamentSettings;
        settings.name = this.tournament.name;
        settings.numLanes = this.tournament.numLanes;
        settings.open();
    }

    saveSettings(): void {
        let settings = this.$.tournamentSettings;
        this.hub.requestUpdateTournament(this.tournament.id, settings.name, settings.numLanes);
        settings.close();
    }

    cancelEditSettings(): void {
        this.$.tournamentSettings.close();
    }
}

RlmSettings.register();