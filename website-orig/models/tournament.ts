enum TournamentState {
    PreEvent,
    EventReady,
    ReadyToRace,
    RaceInProgress,
    RaceFinalized,
    PostEvent
}

class Tournament {
    id: number;
    name: string;
    numLanes: number;
    state: TournamentState;
    cars: Array<Car>;
    races: Array<Race>;

    static fromPayload(payload: any): Tournament {
        let tournament = new Tournament();

        tournament.id = payload.ID;
        tournament.name = payload.Name;
        tournament.numLanes = payload.NumLanes;
        tournament.state = payload.State;
        tournament.cars = Car.getCarsFromPayload(payload.Cars);
        tournament.races = Race.getRacesFromPayload(payload.Races);

        return tournament;
    }

    static getTournamentsFromPayload(payload: any): Array<Tournament> {
        let tournaments: Array<Tournament> = new Array<Tournament>();

        for (let t of payload) {
            tournaments.push(Tournament.fromPayload(t));
        }

        return tournaments;
    }
}