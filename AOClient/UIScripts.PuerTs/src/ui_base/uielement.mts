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

    onOpen() {
    }

    onClose() {
    }

    addComponent<T extends ElementComponent>(TClass:Function & { prototype : T, new(ui:UIElement):T }) {
        let component:T = new TClass(this);
        this.components.set(typeof(component), component);
        component.awake();
    }

    removeComponent(component:ElementComponent) {
        component.dispose();
        this.components.delete(typeof(component));
    }

    dispose(){
        for (let component of this.components.values()) {
            component.dispose();
        }
        this.components.clear();
        this.gobj.Dispose();
    }
}
