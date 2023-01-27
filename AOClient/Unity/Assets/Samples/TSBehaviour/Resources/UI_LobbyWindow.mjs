/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_WindowFrame from "./UI_WindowFrame.mjs";
import "csharp";
var fgui = CS.FairyGUI;
export default class UI_LobbyWindow {
    constructor(GObject) {
        this.GObject = GObject;
        this.GComponent = GObject.asCom;
        this.m_frame = new UI_WindowFrame(this.GComponent.GetChildAt(0));
        this.m_enterBtn = this.GComponent.GetChildAt(1);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "LobbyWindow"));
        var ui = new UI_LobbyWindow(GObject);
        return ui;
    }
}
UI_LobbyWindow.URL = "ui://hlimh2ngm78f4";
