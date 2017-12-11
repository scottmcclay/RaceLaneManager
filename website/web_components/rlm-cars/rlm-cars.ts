@component("rlm-cars")
class RlmCars extends polymer.Base implements polymer.Element {

    @property({ type: RlmHub, notify: true, reflectToAttribute: true })
    hub: RlmHub;

    @property({ type: Number, notify: true, reflectToAttribute: true })
    tournamentId: number;

    @property({ type: [Car], notify: true})
    cars: Array<Car>;

    private actionIsAdd: boolean = false;

    ready(): void {
        console.log("rlm-cars.ready");
        let editDialog = this.$.rlmEditCar;
        editDialog.addEventListener(RlmEditCar.SAVE_TAPPED, this.saveCar.bind(this));
        editDialog.addEventListener(RlmEditCar.CANCEL_TAPPED, this.cancelEditCar.bind(this));
    }

    @observe('hub')
    hubChanged(): void {
        console.log("rlm-cars.hubChanged");
        if (this.hub) {
            this.hub.on(RlmHub.HUB_READY, this.hubReady, this);
            this.hub.on(RlmHub.GET_CARS_RESPONSE, this.getCarsResponse, this);
            this.hub.on(RlmHub.CARS_UPDATED, this.carsUpdated, this);
            this.hub.on(RlmHub.CAR_UPDATED, this.carUpdated, this);
        }

        this.getCars();
    }

    hubReady(): void {
        console.log("rlm-cars.hubReady");
        this.getCars();
    }

    @observe('tournamentId')
    tournamentIdChanged(): void {
        console.log("rlm-cars.tournamentIdChagned");
        this.getCars();
    }

    private getCars(): void {
        console.log("rlm-cars.getCars");
        this.cars = [];

        if (this.tournamentId && this.hub) {
            console.log("rlm-cars.getCars: calling requestGetCars");
            this.hub.requestGetCars(this.tournamentId);
        }
    }

    getCarsResponse(cars: Array<Car>): void {
        this.set('cars', cars);
    }

    carsUpdated(e: RlmCarsUpdatedEvent): void {
        if (e.tournamentId === this.tournamentId) {
            this.set('cars', e.cars);
        }
    }

    carUpdated(car: Car): void {

    }

    addCar(): void {
        let editDialog = this.$.rlmEditCar;
        editDialog.title = "Add Car";
        this.actionIsAdd = true;
        editDialog.car = undefined;
        editDialog.open();
    }

    saveCar(): void {
        let editDialog: RlmEditCar = this.$.rlmEditCar as RlmEditCar;
        let car: Car = editDialog.car;
        
        try {
            if (this.actionIsAdd) {
                this.hub.requestAddCar(this.tournamentId, car);
            }
            else {
                this.hub.requestUpdateCar(this.tournamentId, car);
            }

            editDialog.close();
        }
        catch (e) {
            editDialog.showErrorMessage(e);
        }
    }

    cancelEditCar(): void {
        this.$.rlmEditCar.close();
    }
}

RlmCars.register();