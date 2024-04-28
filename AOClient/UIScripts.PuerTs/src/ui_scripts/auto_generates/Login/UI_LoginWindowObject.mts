/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import UI_CommonWindowFrame from "../Common/UI_CommonWindowFrame.mjs";
import UI_InputComponent from "./UI_InputComponent.mjs";

import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
import UIRoot from "../../uiroot.mjs";
export default class UI_LoginWindowObject  extends UIWindow  {

	public g_frame:UI_CommonWindowFrame;
	public g_loginBtn:fgui.GButton;
	public g_accountInput:UI_InputComponent;
	public g_passwordInput:UI_InputComponent;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngwewl5";

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_frame = new UI_CommonWindowFrame(this.GComponent.GetChildAt(0));
		this.g_loginBtn = (this.GComponent.GetChildAt(1) as fgui.GButton);
		this.g_accountInput = new UI_InputComponent(this.GComponent.GetChildAt(2));
		this.g_passwordInput = new UI_InputComponent(this.GComponent.GetChildAt(3));
	}
}