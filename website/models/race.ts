class Race {
    raceNum: number;
    laneAssignments: Array<LaneAssignment>;

    static fromPayload(payload: any): Race {
        return {
            raceNum: payload.RaceNumber,
            laneAssignments: LaneAssignment.getLaneAssignmentsFromPayload(payload.LaneAssignments)
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

