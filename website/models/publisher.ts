interface ISubscription {
    callback: Function;
    context: any;
}

class Subscription implements ISubscription {
    callback: Function;
    context: any;

    constructor(c: Function, ctx: any) {
        this.callback = c;
        this.context = ctx;
    }
}

interface IPublisher {
    on(event: string, handler: Function, context: Object): void;
    off(event: string, handler: Function, context: Object): void;
}

class Publisher implements IPublisher {
    private subscriptions: { [event: string]: Array<Subscription> | undefined } = { };

    on(event: string, handler: Function, context: Object): void {
        if (this.subscriptions[event] === undefined) {
            this.subscriptions[event] = new Array<Subscription>();
        }

        // make sure the function is not already registered
        for(let sub of this.subscriptions[event]) {
            if ((sub.callback === handler) && (sub.context === context)) {
                return;
            }
        }

        this.subscriptions[event].push(new Subscription(handler, context));
    }

    off(event: string, handler: Function, context: Object): void {
        if (this.subscriptions[event] !== undefined) {

            // find the subscription
            for(let i = 0; i < this.subscriptions[event].length; i++) {
                let sub = this.subscriptions[event][i];
                if ((sub.callback === handler) && (sub.context === context)) {
                    this.subscriptions[event].splice(i, 1);
                    break;
                }
            }
        }
    }

    fire(event: string, payload?: any): void {
        if (this.subscriptions[event] !== undefined) {
            for(let sub of this.subscriptions[event]) {
                sub.callback.call(sub.context, payload);
            }
        }
    }
}
