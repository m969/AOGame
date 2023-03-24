import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
import UIElement from "./uielement.mjs";
var fgui = CS.FairyGUI;
export default class UIWindow extends UIElement {
    constructor(GObject) {
        super(GObject);
        this.window = new fgui.Window();
        this.window.contentPane = this.gobj.asCom;
    }
    showWindow(parent) {
        UIRoot.Windows.set(this.constructor.name, this);
        parent.ShowWindow(this.window);
        this.window.MakeFullScreen();
    }
    dispose() {
        UIRoot.Windows.delete(typeof (this));
        this.components.clear();
        this.window.Dispose();
    }
}
