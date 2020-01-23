import { Car } from './car';

export class Standing {
    car: Car;
    position: string;
    points: number;
    averageTime: number;
    averageTimeSeconds: number;

    static fromPayload(payload: any): Standing {
        return {
            car: Car.fromPayload(payload.Car),
            position: payload.Position,
            points: payload.Points,
            averageTime: payload.AverageTime,
            averageTimeSeconds: payload.AverageTime / 1000
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