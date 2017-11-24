@component("rlm-app")
class RlmApp extends polymer.Base implements polymer.Element {

    @property({ notify: true })
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

            case 'settings': 'Settings';
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
        this.hub.on('proxyReady', this.proxyReady, this);
        this.hub.on('tournamentsUpdated', this.tournamentsUpdated, this);
        this.hub.on('getTournamentsResponse', this.getTournamentsResponse, this);
    }

    proxyReady(): void {
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
        this.tournaments = tournaments;
    }

    newTournamentTap(): void {
        this.hub.createTournament('New Tournament', 4);
        // this.$.createTournamentName.value = "";
        // this.$.createTournamentNumLanes.value = 4;
        // this.$.createTournamentErrorMessage.innerText = "";
        // this.$.createTournamentDialog.open();
    }

    doCreateTournament(): void {
        this.hub.createTournament(this.$.createTournamentName.value, this.$.createTournamentNumLanes.value);
        // let createTournamentAjax = this.$.createTournamentAjax;
        // createTournamentAjax.body = { "name": this.$.createTournamentName.value, "lanes": this.$.createTournamentNumLanes.value };
        // createTournamentAjax.generateRequest();
    }

    tournamentsUpdated(tournaments: Array<Tournament>): void {
        //console.log('Tournament added to host: name = ' + tournament.name);
        this.displayTournaments(tournaments);
    }

    tournamentsPostResponse(e: any): void {
        let newTournament = e.detail.response;

        this.push('tournaments', newTournament);
        this.$.createTournamentDialog.close();

        setTimeout(function (ctx: any) {
            // use a timeout to allow the list of tournaments to re-populate dom elements, then select the new tournament
            ctx.set('route.path', '/tournament/' + newTournament.ID + '/settings');
        }, 10, this);
    }

    tournamentsPostError(e: any): void {
        this.$.createTournamentErrorMessage.innerText = "Unable to create tournament";
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

    tournamentsResponse(e: any): void {
        //this.tournaments = e.detail.response;
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