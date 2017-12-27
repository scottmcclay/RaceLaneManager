enum RaceState {
    NotStarted,
    Racing,
    Done
}

class Race {
    raceNum: number;
    state: RaceState;
    laneAssignments: Array<LaneAssignment>;

    static fromPayload(payload: any): Race {
        return {
            raceNum: payload.RaceNumber,
            state: payload.State,
            laneAssignments: LaneAssignment.getLaneAssignmentsFromPayload(payload.LaneAssignments)
        };
    }

    static toPayload(race: Race): any {
        return {
            'RaceNumber': race.raceNum,
            'State': race.state,
            'LaneAssignments': race.laneAssignments
        };
    }
    
    static getRacesFromPayload(payload: any): Array<Race> {
        let races: Array<Race> = new Array<Race>();
        
        for (let r of payload) {
            races.push(Race.fromPayload(r));
        }

        return races;
    }
}

