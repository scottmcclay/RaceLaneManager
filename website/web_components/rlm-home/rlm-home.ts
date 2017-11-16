@component('rlm-home')
class RlmHome extends polymer.Base implements polymer.Element {

    private connection: SignalR.Hub.Connection;
    private proxy: SignalR.Hub.Proxy;

    @property({ type: Tournament, reflectToAttribute: true, notify: true })
    tournament: Tournament;

    @property({ type: Race, reflectToAttribute: true, notify: true })
    currentRace: Race;

    @property({ type: String })
    testText: string;

    ready() {
        this.connection = $.hubConnection();
        this.proxy = this.connection.createHubProxy("testHub");
        //this.proxy = this.connection.hub.createHubProxy("testHub");
        this.proxy.on('displayMessage', this.displayMessage);
        this.connection.start()
            .done(function () { console.log("Now connected to test hub!!!: "); })
            .fail(function () { console.log("Unable to connect to hub"); });

        this.testText = "Foo";
    }

    test() {
        this.proxy.invoke('Test', 'A message from the client.')
        .done(function () { console.log('Invocation of Test method succeeded.'); })
        .fail(function (error) { console.log('Invocation of Test method failed. Error: ' + error); });
    }

    displayMessage(message: string) {
        console.log('Received message from server: ' + message);
    }
        
}

RlmHome.register();