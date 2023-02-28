/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import UI_WindowFrame from "./UI_WindowFrame.mjs";

import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
import UIRoot from "../../uiroot.mjs";
export default class UI_LoginWindow  extends UIWindow  {

	public g_frame:UI_WindowFrame;
	public g_loginBtn:fgui.GButton;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngwewl5";

	public static createInstance():UI_LoginWindow {
		let GObject = (fgui.UIPackage.CreateObject("Login", "LoginWindow"));
		var ui = new UI_LoginWindow(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_frame = new UI_WindowFrame(this.GComponent.GetChildAt(0));
		this.g_loginBtn = (this.GComponent.GetChildAt(1) as fgui.GButton);
	}
}