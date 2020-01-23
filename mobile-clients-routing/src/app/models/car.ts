export class Car {
    id: number;
    number: number;
    name: string;
    owner: string;
    den: string;

    static fromPayload(payload: any): Car {
        if (payload == null) return null;

        return {
            id: payload.ID,
            number: payload.Number,
            name: payload.Name,
            owner: payload.Owner,
            den: payload.Den
        };
    }
    
    static getCarsFromPayload(payload: any): Array<Car> {
        let cars: Array<Car> = new Array<Car>();
        
        for (let t of payload) {
            cars.push(Car.fromPayload(t));
        }

        return cars;
    }
}