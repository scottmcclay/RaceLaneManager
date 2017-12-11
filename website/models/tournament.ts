class Tournament {
    id: number;
    name: string;
    numLanes: number;
    cars: Array<Car>;
    numRaces: number;
    assignments: Array<RaceLaneAssignment>;

    static fromPayload(payload: any): Tournament {
        let tournament = new Tournament();

        tournament.id = payload.ID;
        tournament.name = payload.Name;
        tournament.numLanes = payload.NumLanes;

        tournament.cars = new Array<Car>();
        for (let c of payload.Cars) {
            tournament.cars.push(Car.fromPayload(c));
        }

        tournament.numRaces = payload.NumRaces;

        tournament.assignments = new Array<RaceLaneAssignment>();
        if (payload.Assignments) {
            for (let a of payload.Assignments) {
                tournament.assignments.push(RaceLaneAssignment.fromPayload(a));
            }
        }

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