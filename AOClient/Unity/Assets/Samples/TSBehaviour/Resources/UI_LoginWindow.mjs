/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";
import "csharp";
var fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
export default class UI_LoginWindow extends UIWindow {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
        this.g_loginBtn = this.GComponent.GetChildAt(1);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "LoginWindow"));
        var ui = new UI_LoginWindow(GObject);
        return ui;
    }
}
UI_LoginWindow.URL = "ui://hlimh2ngwewl5";
