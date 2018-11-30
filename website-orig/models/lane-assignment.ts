class LaneAssignment {
    lane: number;
    car: Car;
    elapsedTime: number;
    position: number;
    points: number;

    static fromPayload(payload: any): LaneAssignment {
        let assignment = new LaneAssignment();

        assignment.lane = payload.Lane;
        assignment.car = Car.fromPayload(payload.Car);
        assignment.elapsedTime = payload.ElapsedTime;
        assignment.position = payload.Position;
        assignment.points = payload.Points;

        return assignment;
    }

    get elapsedTimeSeconds(): number {
        return this.elapsedTime / 1000;
    }

    set elapsedTimeSeconds(value: number) {
        this.elapsedTime = value * 1000;
    }

    static getLaneAssignmentsFromPayload(payload: any): Array<LaneAssignment> {
        let assignments: Array<LaneAssignment> = new Array<LaneAssignment>();
        
        for (let a of payload) {
            assignments.push(LaneAssignment.fromPayload(a));
        }

        return assignments;
    }
}