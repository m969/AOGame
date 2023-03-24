/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import "csharp";
var fgui = CS.FairyGUI;
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_InputComponent extends UIElement {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_inputtext = this.GComponent.GetChildAt(1);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "InputComponent"));
        var ui = new UI_InputComponent(GObject);
        return ui;
    }
}
UI_InputComponent.URL = "ui://hlimh2ngf3pp7";
