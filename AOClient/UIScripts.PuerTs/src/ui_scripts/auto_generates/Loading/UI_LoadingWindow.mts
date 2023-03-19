/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";

import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_LoadingWindow  extends UIWindow  {

	public g_frame:UI_CommonWindowFrame;
	public g_dragArea:fgui.GGraph;
	public g_title:fgui.GTextField;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://51tqrbhhibwr7";

	public static createInstance():UI_LoadingWindow {
		let GObject = (fgui.UIPackage.CreateObject("Loading", "LoadingWindow"));
		var ui = new UI_LoadingWindow(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
		this.g_dragArea = (this.GComponent.GetChildAt(1) as fgui.GGraph);
		this.g_title = (this.GComponent.GetChildAt(2) as fgui.GTextField);
	}
}