import "csharp";
import "puerts";
import ElementComponent from "./element_component.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

export default class UIElement {
    public gobj:fgui.GObject;
    public components:Map<string, ElementComponent>;

    constructor(GObject: fgui.GObject) {
        this.gobj = GObject;
        this.components = new Map();
    }

    addElementComponent<T extends ElementComponent>(TClass:Function & { prototype : T }) {
        let component:T = TClass(this);
        this.components.set(typeof(component), component);
    }

    removeElementComponent(component:ElementComponent) {
        this.components.delete(typeof(component));
    }

    dispose(){
        this.components.clear();
        this.gobj.Dispose();
    }
}
