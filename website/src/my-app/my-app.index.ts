import { PolymerElement, html } from '@polymer/polymer/polymer-element';
import * as view from './my-app.template.html';
import {customElement, property} from '@polymer/decorators';

@customElement('my-app')
export class MyApp extends PolymerElement {

    @property({type: String, reflectToAttribute: true, notify: true})
    name: string = 'Bob';

    static get template() {
        const template = document.createElement('template');
        template.innerHTML = `${view}`;
        return template;
    }

    constructor() {
        super();
    }
}