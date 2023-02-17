/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_WindowFrame from "./UI_WindowFrame.mjs";
import "csharp";
var fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
export default class UI_MainWindow extends UIWindow {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_frame = new UI_WindowFrame(this.GComponent.GetChildAt(0));
        this.g_joystick = this.GComponent.GetChildAt(1);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "MainWindow"));
        var ui = new UI_MainWindow(GObject);
        return ui;
    }
}
UI_MainWindow.URL = "ui://hlimh2ngwewl6";
