/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";
import "csharp";
var fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIRoot from "../../uiroot.mjs";
export default class UI_LoadingWindow extends UIWindow {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
        this.g_loadingProgressBar = this.GComponent.GetChildAt(2);
        this.g_title = this.GComponent.GetChildAt(3);
    }
    static getInstance() {
        return UIRoot.Windows.get("UI_LoadingWindow");
    }
    static createInstance() {
        let GObject = (fgui.UIPackage.CreateObject("Loading", "LoadingWindow"));
        var ui = new UI_LoadingWindow(GObject);
        return ui;
    }
}
UI_LoadingWindow.URL = "ui://51tqrbhhibwr7";
