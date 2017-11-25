@component('rlm-home')
class RlmHome extends polymer.Base implements polymer.Element {

    // private connection: SignalR.Hub.Connection;
    @property({ type: RlmHub, notify: true, reflectToAttribute: true})
    hub: RlmHub;

    @property({ type: Tournament, reflectToAttribute: true, notify: true })
    tournament: Tournament;

    @property({ type: Race, reflectToAttribute: true, notify: true })
    currentRace: Race;

}

RlmHome.register();