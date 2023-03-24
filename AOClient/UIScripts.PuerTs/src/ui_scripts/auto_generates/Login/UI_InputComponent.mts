/** This is an automatically generated class by FairyGUI. Please do not modify it. **/


import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
import UIRoot from "../../uiroot.mjs";
export default class UI_InputComponent  extends UIElement  {

	public g_inputtext:fgui.GTextInput;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngf3pp7";

	public static createInstance():UI_InputComponent {
		let GObject = (fgui.UIPackage.CreateObject("Login", "InputComponent"));
		var ui = new UI_InputComponent(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_inputtext = (this.GComponent.GetChildAt(1) as fgui.GTextInput);
	}
}