import "csharp";
import "puerts";
export default class UIElement {
    constructor(GObject) {
        this.gobj = GObject;
        this.components = new Map();
    }
    addElementComponent(TClass) {
        let component = TClass(this);
        this.components.set(typeof (component), component);
    }
    removeElementComponent(component) {
        this.components.delete(typeof (component));
    }
    dispose() {
        this.components.clear();
        this.gobj.Dispose();
    }
}
