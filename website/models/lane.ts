class Lane {
    laneNum: number;
    active: boolean;

    static fromPayload(payload: any): Lane {
        return {
            laneNum: payload.LaneNum,
            active: payload.Active
        };
    }
}