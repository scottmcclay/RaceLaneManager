@component('rlm-edit-race-dialog')
class RlmEditRaceDialog extends polymer.Base implements polymer.Element {
    static readonly SAVE_TAPPED: string = "rlmEditRaceDialogSaveTapped";
    static readonly CANCEL_TAPPED: string = "rlmEditRaceDialogCancelTapped";

    @property({ type: String, notify: true, reflectToAttribute: true })
    title: string;

    @property({ type: String, notify: true, reflectToAttribute: true, value: 'Save' })
    saveText: string;

    @property({ type: String, notify: true, reflectToAttribute: true, value: 'Cancel' })
    cancelText: string;

    race: Race;

    @property({ type: String, notify: true })
    errorMessage: string;

    open(): void {
        let dialog = this.$.editRaceDialog;
        let body = document.querySelector("body");
        body.appendChild(dialog);
        dialog.open();
    }

    close(): void {
        let dialog = this.$.editRaceDialog;
        let holder = this.$.rlmEditRaceDialogHolder;
        holder.appendChild(dialog);
        dialog.close();
    }

    saveTapped(): void {
        this.fire(RlmEditRaceDialog.SAVE_TAPPED);
    }

    cancelTapped(): void {
        this.fire(RlmEditRaceDialog.CANCEL_TAPPED);
    }

    showErrorMessage(message: string): void {
        this.errorMessage = message;
    }
}

RlmEditRaceDialog.register();