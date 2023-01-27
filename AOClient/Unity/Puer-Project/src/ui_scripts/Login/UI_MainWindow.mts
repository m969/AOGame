/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import UI_WindowFrame from "./UI_WindowFrame.mjs";

import "csharp";
import fgui = CS.FairyGUI;
export default class UI_MainWindow {

	public m_frame:UI_WindowFrame;
	public m_joystick:fgui.GGraph;
	public GObject:fgui.GObject;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngwewl6";

	public static createInstance():UI_MainWindow {
		let GObject = (fgui.UIPackage.CreateObject("Login", "MainWindow"));
		var ui = new UI_MainWindow(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		this.GObject = GObject;
		this.GComponent = GObject.asCom;
		this.m_frame = new UI_WindowFrame(this.GComponent.GetChildAt(0));
		this.m_joystick = (this.GComponent.GetChildAt(1) as fgui.GGraph);
	}
}