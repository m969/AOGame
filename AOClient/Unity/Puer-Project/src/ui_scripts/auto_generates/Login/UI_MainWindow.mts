/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import UI_WindowFrame from "./UI_WindowFrame.mjs";

import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_MainWindow  extends UIWindow  {

	public g_frame:UI_WindowFrame;
	public g_joystick:fgui.GGraph;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngwewl6";

	public static createInstance():UI_MainWindow {
		let GObject = (fgui.UIPackage.CreateObject("Login", "MainWindow"));
		var ui = new UI_MainWindow(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_frame = new UI_WindowFrame(this.GComponent.GetChildAt(0));
		this.g_joystick = (this.GComponent.GetChildAt(1) as fgui.GGraph);
	}
}