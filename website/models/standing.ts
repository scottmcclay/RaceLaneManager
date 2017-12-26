class Standing {
    car: Car;
    position: string;
    points: number;

    static fromPayload(payload: any): Standing {
        return {
            car: Car.fromPayload(payload.Car),
            position: payload.Position,
            points: payload.Points
        };
    }
    
    static getStandingsFromPayload(payload: any): Array<Standing> {
        let standings: Array<Standing> = new Array<Standing>();
        
        for (let s of payload) {
            standings.push(Standing.fromPayload(s));
        }

        return standings;
    }
}