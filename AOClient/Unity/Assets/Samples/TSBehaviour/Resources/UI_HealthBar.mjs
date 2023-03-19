/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import "csharp";
var fgui = CS.FairyGUI;
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_HealthBar extends UIElement {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_tipsText = this.GComponent.GetChildAt(2);
        this.g_t0 = this.GComponent.GetTransitionAt(0);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Common", "HealthBar"));
        var ui = new UI_HealthBar(GObject);
        return ui;
    }
}
UI_HealthBar.URL = "ui://bch3lpzzdpkf7";
