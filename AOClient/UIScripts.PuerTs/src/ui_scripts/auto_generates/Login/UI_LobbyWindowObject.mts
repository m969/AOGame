/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";

import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
import UIRoot from "../../uiroot.mjs";
export default class UI_LobbyWindowObject  extends UIWindow  {

	public g_frame:UI_CommonWindowFrame;
	public g_enterBtn:fgui.GButton;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngm78f4";

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
		this.g_enterBtn = (this.GComponent.GetChildAt(1) as fgui.GButton);
	}
}