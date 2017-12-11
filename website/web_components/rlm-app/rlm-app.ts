@component("rlm-app")
class RlmApp extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true })
    hub: RlmHub;

    @property({ reflectToAttribute: true, notify: true })
    tournaments: Array<Tournament>;

    @property({ type: Tournament, reflectToAttribute: true, notify: true})
    selectedTournament: Tournament;
    
    @property({type: Boolean, reflectToAttribute: true})
    isAdmin: boolean;

    @property({type: String})
    viewName: string;

    @observe('routeData.tournamentId')
    routeTournamentIdChanged(newId: number, oldId: number) {
        let tournament: Tournament = this.getTournamentById(newId);
        this.selectedTournament = tournament;
        
        if ((tournament !== null) && ((this.selectedTournament === null) || (this.selectedTournament.id != tournament.id))) {
            this.set('subrouteData.view', 'home');
        }

        this.$.drawer.close();
    }

    @observe('subrouteData.view')
    subrouteViewChanged(newView: string, oldView: string) {
        switch(newView) {
            case 'cars':
            this.viewName = 'Cars';
            break;

            case 'races':
            this.viewName = 'Races';
            break;

            case 'settings':
            this.viewName = 'Settings';
            break;

            case 'home':
            default:
            this.viewName = 'Home';
            break;
        }
    }

    ready(): void {
        this.isAdmin = this.checkAdminCookie();
        //let connection: SignalR.Hub.Connection = this.$.hubConnection();
        this.hub = new RlmHub();
        this.hub.initialize($.hubConnection());
        this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
        this.hub.on(RlmHub.TOURNAMENTS_UPDATED, this.tournamentsUpdated, this);
        this.hub.on(RlmHub.GET_TOURNAMENTS_RESPONSE, this.getTournamentsResponse, this);
        this.hub.on(RlmHub.TOURNAMENT_UPDATED, this.tournamentUpdated, this);
    }

    hubReady(): void {
        this.hub.requestGetTournaments();
    }

    checkAdminCookie(): boolean {
        let cookies = document.cookie.split(';');
        let cookieName: string = 'admin=';
        let cookieValue: string = null;

        for (var i = 0; i < cookies.length; i++) {
            let cookie: string = cookies[i];
            while (cookie.charAt(0) == ' ') {
                cookie = cookie.substring(1);
            }

            if (cookie.indexOf(cookieName) == 0) {
                cookieValue = cookie.substring(cookieName.length, cookie.length);
                break;
            }
        }

        if (cookieValue == null) {
            return false;
        }

        return (cookieValue == 'true');
    }

    setAdminCookie(): void {
        document.cookie = 'admin=true';
    }

    deleteAdminCookie(): void {
        document.cookie = 'admin=; expires=Thu, 01 Jan 1970 00:00:00 GMT';
    }

    toggleDrawer(): void {
        this.$.drawer.toggle();
    }

    homeTap(): void {
        this.navigate('home');
    }

    racesTap(): void {
        this.navigate('races');
    }

    carsTap(): void {
        this.navigate('cars');
    }

    settingsTap(): void {
        this.navigate('settings');
    }

    navigate(view: string): void {
        if (this.selectedTournament == null) {
            this.set('route.path', '/');
        }
        else {
            this.set('route.path', '/tournament/' + this.selectedTournament.id + '/' + view);
        }
    }

    getTournamentsResponse(tournaments: Array<Tournament>): void {
        this.displayTournaments(tournaments);
    }

    private displayTournaments(tournaments: Array<Tournament>): void {
        // override Polymer's dirty checking
        let temp = tournaments;
        this.tournaments = [];
        this.tournaments = temp;
    }

    newTournamentTap(): void {
        this.hub.createTournament('New Tournament', 4);
    }

    tournamentsUpdated(tournaments: Array<Tournament>): void {
        this.displayTournaments(tournaments);
    }

    tournamentUpdated(tournament: Tournament): void {
        let index: number;
        for (index = 0; index < this.tournaments.length; index++) {
            if (this.tournaments[index].id === tournament.id) {
                this.splice('tournaments', index, 1, tournament);
                break;
            }
        }
        this.displayTournaments(this.tournaments);
    }

    signInTap(): void {
        let signInDialog = this.$.signInDialog;
        signInDialog.open();
    }

    doSignIn(): void {
        let request: any = this.$.authAjax;
        request.body = { "name": this.$.authName.value, "password": this.$.authPassword.value };
        request.generateRequest();
    }

    authResponse(e: any): void {
        this.isAdmin = true;
        this.setAdminCookie();
        this.$.signInDialog.close();
    }

    authError(e: any): void {
        this.$.authErrorMessage.innerText = "Unable to sign-in.";
    }

    signOutTap(): void {
        this.deleteAdminCookie();
        this.isAdmin = false;
    }

    getTournamentById(tournamentId: number): Tournament {
        if (this.tournaments != null) {
            for (var i = 0; i < this.tournaments.length; i++) {
                if (this.tournaments[i].id == tournamentId) {
                    return this.tournaments[i];
                }
            }
        }

        return null;
    }

    showPage404(): void {
        //this.page = 'view404';
    }

    showPanel404(): void {
        //this.panel = 'view404';
    }
}

RlmApp.register();