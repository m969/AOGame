/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";
import "csharp";
var fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIRoot from "../../uiroot.mjs";
export default class UI_MainWindow extends UIWindow {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
        this.g_joystick = this.GComponent.GetChildAt(1);
        this.g_skillCureBtn = this.GComponent.GetChildAt(4);
    }
    static getInstance() {
        return UIRoot.Windows.get("UI_MainWindow");
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "MainWindow"));
        var ui = new UI_MainWindow(GObject);
        return ui;
    }
}
UI_MainWindow.URL = "ui://hlimh2ngwewl6";
