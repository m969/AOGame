import "csharp";
import "puerts";
import Component from "./component.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

export default class UIElement {

    public GObject:fgui.GObject;
    public components:Map<string, Component>;

    constructor(GObject: fgui.GObject) {
        this.GObject = GObject;
        this.components = new Map();
    }

    addComponent<T extends Component>(TClass:Function & { prototype : T }) {
        let component:T = TClass(this);
        this.components.set(typeof(component), component);
    }

    removeComponent(component:Component) {
        this.components.delete(typeof(component));
    }

    dispose(){
        this.components.clear();
        this.GObject.Dispose();
    }
}
