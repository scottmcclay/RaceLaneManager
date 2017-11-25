class Car {
    id: number;
    name: string;
    owner: string;
    den: string;
    carNumber: number;

    static fromPayload(payload: any): Car {
        return {
            id: payload.ID,
            name: payload.Name,
            owner: payload.Owner,
            den: payload.Den,
            carNumber: payload.CarNumber
        };
    }
}