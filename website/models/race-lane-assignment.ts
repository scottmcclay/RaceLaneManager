class RaceLaneAssignment {
    raceNum: number;
    lane: Lane;
    car: Car;
    elapsedTime: number;

    static fromPayload(payload: any): RaceLaneAssignment {
        let assignment = new RaceLaneAssignment();

        assignment.raceNum = payload.RaceNum;
        assignment.lane = Lane.fromPayload(payload.Lane);
        assignment.car = Car.fromPayload(payload.Car);
        assignment.elapsedTime = payload.ElapsedTime;

        return assignment;
    }
}