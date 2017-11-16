@component("rlm-app")
class RlmApp extends polymer.Base implements polymer.Element {

    @property({ type: [Tournament], reflectToAttribute: true, notify: true })
    tournaments: Array<Tournament>;

    @property({type: Boolean, reflectToAttribute: true})
    isAdmin: boolean;
}