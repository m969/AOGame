/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";
import "csharp";
import UIWindow from "../../../ui_base/uiwindow.mjs";
export default class UI_MainWindowObject extends UIWindow {
    constructor(GObject) {
        super(GObject);
        this.GComponent = GObject.asCom;
        this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
        this.g_joystick = this.GComponent.GetChildAt(1);
        this.g_skillCureBtn = this.GComponent.GetChildAt(4);
    }
}
UI_MainWindowObject.URL = "ui://hlimh2ngwewl6";
