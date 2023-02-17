/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import "csharp";
var fgui = CS.FairyGUI;
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_WindowFrame extends UIElement {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_contentArea = this.GComponent.GetChildAt(0);
        this.g_dragArea = this.GComponent.GetChildAt(1);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "WindowFrame"));
        var ui = new UI_WindowFrame(GObject);
        return ui;
    }
}
UI_WindowFrame.URL = "ui://hlimh2ngm78f3";
