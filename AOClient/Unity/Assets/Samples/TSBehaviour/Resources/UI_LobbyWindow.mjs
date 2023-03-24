/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";
import "csharp";
var fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIRoot from "../../uiroot.mjs";
export default class UI_LobbyWindow extends UIWindow {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
        this.g_enterBtn = this.GComponent.GetChildAt(1);
    }
    static getInstance() {
        return UIRoot.Windows.get("UI_LobbyWindow");
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Login", "LobbyWindow"));
        var ui = new UI_LobbyWindow(GObject);
        return ui;
    }
}
UI_LobbyWindow.URL = "ui://hlimh2ngm78f4";
