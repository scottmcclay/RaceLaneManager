@component('rlm-tournament-settings')
class RlmTournamentSettings extends polymer.Base implements polymer.Element {
    static readonly SAVE_TAPPED: string = "saveTapped";
    static readonly CANCEL_TAPPED: string = "cancelTapped";

    @property({ type: String, notify: true, reflectToAttribute: true })
    title: string;

    @property({ type: String, notify: true, reflectToAttribute: true, value: 'Save' })
    saveText: string;

    @property({ type: String, notify: true, reflectToAttribute: true, value: 'Cancel' })
    cancelText: string;

    @property({ type: String, notify: true, reflectToAttribute: true })
    name: string;

    @property({ type: Number, notify: true, reflectToAttribute: true })
    numLanes: number;

    open(): void {
        let dialog = this.$.editTournamentDialog;
        let body = document.querySelector("body");
        body.appendChild(dialog);
        dialog.open();
    }

    close(): void {
        let dialog = this.$.editTournamentDialog;
        let holder = this.$.rlmSettingsDialogHolder;
        holder.appendChild(dialog);
        dialog.close();
    }

    saveTapped(): void {
        this.fire(RlmTournamentSettings.SAVE_TAPPED);
    }

    cancelTapped(): void {
        this.fire(RlmTournamentSettings.CANCEL_TAPPED);
    }
}

RlmTournamentSettings.register();