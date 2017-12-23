class LaneAssignment {
    lane: number;
    car: Car;
    elapsedTime: number;

    static fromPayload(payload: any): LaneAssignment {
        let assignment = new LaneAssignment();

        assignment.lane = payload.Lane;
        assignment.car = Car.fromPayload(payload.Car);
        assignment.elapsedTime = payload.ElapsedTime;

        return assignment;
    }

    static getLaneAssignmentsFromPayload(payload: any): Array<LaneAssignment> {
        let assignments: Array<LaneAssignment> = new Array<LaneAssignment>();
        
        for (let a of payload) {
            assignments.push(LaneAssignment.fromPayload(a));
        }

        return assignments;
    }
}