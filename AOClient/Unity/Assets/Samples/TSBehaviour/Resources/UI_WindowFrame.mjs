/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import "csharp";
var fgui = CS.FairyGUI;
export default class UI_WindowFrame {
    constructor(GObject) {
        this.GObject = GObject;
        this.GComponent = GObject.asCom;
        this.m_contentArea = this.GComponent.GetChildAt(0);
        this.m_dragArea = this.GComponent.GetChildAt(1);
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "WindowFrame"));
        var ui = new UI_WindowFrame(GObject);
        return ui;
    }
}
UI_WindowFrame.URL = "ui://hlimh2ngm78f3";
