class Tournament {
    id: number;
    date: string;
    name: string;
    lanes: [Lane];
    numLanes: number;
    cars: [Car];
    numRaces: number;
    assignments: [RaceLaneAssignment];
}