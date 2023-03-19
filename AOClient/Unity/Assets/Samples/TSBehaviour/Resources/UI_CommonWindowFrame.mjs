/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import "csharp";
var fgui = CS.FairyGUI;
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_CommonWindowFrame extends UIElement {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_c1 = this.GComponent.GetControllerAt(0);
        this.g_contentArea = this.GComponent.GetChildAt(0);
        this.g_dragArea = this.GComponent.GetChildAt(1);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Common", "CommonWindowFrame"));
        var ui = new UI_CommonWindowFrame(GObject);
        return ui;
    }
}
UI_CommonWindowFrame.URL = "ui://bch3lpzzm78f3";
