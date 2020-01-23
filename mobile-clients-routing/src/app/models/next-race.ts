import { NextRaceLaneAssignment } from './next-race-lane-assignment';

export class NextRace {
    raceNum: number;
    lanes: Array<NextRaceLaneAssignment>;

    constructor() {
        this.lanes = new Array<NextRaceLaneAssignment>();
    }
}