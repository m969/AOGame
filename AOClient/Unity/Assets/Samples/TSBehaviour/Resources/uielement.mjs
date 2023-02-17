import "csharp";
import "puerts";
export default class UIElement {
    constructor(GObject) {
        this.GObject = GObject;
        this.components = new Map();
    }
    addComponent(TClass) {
        let component = TClass(this);
        this.components.set(typeof (component), component);
    }
    removeComponent(component) {
        this.components.delete(typeof (component));
    }
    dispose() {
        this.components.clear();
        this.GObject.Dispose();
    }
}
