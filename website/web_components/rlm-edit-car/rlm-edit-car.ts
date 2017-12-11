@component('rlm-edit-car')
class RlmEditCar extends polymer.Base implements polymer.Element {
    static readonly SAVE_TAPPED: string = "rlmEditCarSaveTapped";
    static readonly CANCEL_TAPPED: string = "rlmEditCarCancelTapped";

    @property({ type: String, notify: true, reflectToAttribute: true })
    title: string;

    @property({ type: String, notify: true, reflectToAttribute: true, value: 'Save' })
    saveText: string;

    @property({ type: String, notify: true, reflectToAttribute: true, value: 'Cancel' })
    cancelText: string;

    carId: number;
    
    @property({ type: String, notify: true, reflectToAttribute: true })
    carName: string;

    @property({ type: String, notify: true, reflectToAttribute: true })
    carNumber: string;

    @property({ type: String, notify: true, reflectToAttribute: true })
    carOwner: string;

    @property({ type: String, notify: true, reflectToAttribute: true })
    carDen: string;

    @property({ type: String, notify: true })
    errorMessage: string;

    get car(): Car {
        return {
            id: this.carId,
            name: this.carName,
            carNumber: Number(this.carNumber),
            owner: this.carOwner,
            den: this.carDen
        }
    }

    set car(car: Car) {
        if (car) {
            this.carId
            this.set('carName', car.name);
            this.set('carNumber', car.carNumber.toString);
            this.set('carOwner', car.owner);
            this.set('carDen', car.den);
        }
        else {
            this.carId = undefined;
            this.set('carName', '');
            this.set('carNumber', '');
            this.set('carOwner', '');
            this.set('carDen', '');
        }
    }

    open(): void {
        let dialog = this.$.editCarDialog;
        let body = document.querySelector("body");
        body.appendChild(dialog);
        dialog.open();
    }

    close(): void {
        let dialog = this.$.editCarDialog;
        let holder = this.$.rlmEditCarDialogHolder;
        holder.appendChild(dialog);
        dialog.close();
    }

    saveTapped(): void {
        this.fire(RlmEditCar.SAVE_TAPPED);
    }

    cancelTapped(): void {
        this.fire(RlmEditCar.CANCEL_TAPPED);
    }

    showErrorMessage(message: string): void {
        this.errorMessage = message;
    }
}

RlmEditCar.register();