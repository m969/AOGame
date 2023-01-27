/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_WindowFrame from "./UI_WindowFrame.mjs";
import "csharp";
var fgui = CS.FairyGUI;
export default class UI_MainWindow {
    constructor(GObject) {
        this.GObject = GObject;
        this.GComponent = GObject.asCom;
        this.m_frame = new UI_WindowFrame(this.GComponent.GetChildAt(0));
        this.m_joystick = this.GComponent.GetChildAt(1);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "MainWindow"));
        var ui = new UI_MainWindow(GObject);
        return ui;
    }
}
UI_MainWindow.URL = "ui://hlimh2ngwewl6";
