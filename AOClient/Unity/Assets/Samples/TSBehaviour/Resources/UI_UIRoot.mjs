/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import "csharp";
var fgui = CS.FairyGUI;
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_UIRoot extends UIElement {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_UIHide = this.GComponent.GetChildAt(0);
        this.g_UIBack = this.GComponent.GetChildAt(1);
        this.g_UIMidd = this.GComponent.GetChildAt(2);
        this.g_UIFront = this.GComponent.GetChildAt(3);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Common", "UIRoot"));
        var ui = new UI_UIRoot(GObject);
        return ui;
    }
}
UI_UIRoot.URL = "ui://bch3lpzzj3ot8";
