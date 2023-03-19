import "csharp";
import "puerts";
export default class UIElement {
    constructor(GObject) {
        this.gobj = GObject;
        this.components = new Map();
    }
    addComponent(TClass) {
        let component = new TClass(this);
        this.components.set(typeof (component), component);
        component.awake();
    }
    removeComponent(component) {
        component.dispose();
        this.components.delete(typeof (component));
    }
    dispose() {
        for (let component of this.components.values()) {
            component.dispose();
        }
        this.components.clear();
        this.gobj.Dispose();
    }
}
