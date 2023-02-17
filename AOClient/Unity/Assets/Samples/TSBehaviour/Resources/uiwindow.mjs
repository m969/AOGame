import "csharp";
import "puerts";
import UIElement from "./uielement.mjs";
var fgui = CS.FairyGUI;
export default class UIWindow extends UIElement {
    constructor(GObject) {
        super(GObject);
        this.window = new fgui.Window();
        this.window.contentPane = this.GObject.asCom;
    }
    showWindow() {
        this.window.Show();
        this.window.MakeFullScreen();
    }
    dispose() {
        this.components.clear();
        this.window.Dispose();
    }
}
