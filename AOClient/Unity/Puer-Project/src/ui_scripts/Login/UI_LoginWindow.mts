/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import UI_WindowFrame from "./UI_WindowFrame.mjs";

import "csharp";
import fgui = CS.FairyGUI;
export default class UI_LoginWindow {

	public m_frame:UI_WindowFrame;
	public m_loginBtn:fgui.GButton;
	public GObject:fgui.GObject;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngwewl5";

	public static createInstance():UI_LoginWindow {
		let GObject = (fgui.UIPackage.CreateObject("Login", "LoginWindow"));
		var ui = new UI_LoginWindow(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		this.GObject = GObject;
		this.GComponent = GObject.asCom;
		this.m_frame = new UI_WindowFrame(this.GComponent.GetChildAt(0));
		this.m_loginBtn = (this.GComponent.GetChildAt(1) as fgui.GButton);
	}
}