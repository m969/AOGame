/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";
import "csharp";
import UIWindow from "../../../ui_base/uiwindow.mjs";
export default class UI_LobbyWindowObject extends UIWindow {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
        this.g_enterBtn = this.GComponent.GetChildAt(1);
    }
}
UI_LobbyWindowObject.URL = "ui://hlimh2ngm78f4";
