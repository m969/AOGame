/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import "csharp";
import fgui = CS.FairyGUI;
export default class UI_WindowFrame {

	public m_contentArea:fgui.GGraph;
	public m_dragArea:fgui.GGraph;
	public GObject:fgui.GObject;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngm78f3";

	public static createInstance():UI_WindowFrame {
		let GObject = (fgui.UIPackage.CreateObject("Login", "WindowFrame"));
		var ui = new UI_WindowFrame(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		this.GObject = GObject;
		this.GComponent = GObject.asCom;
		this.m_contentArea = (this.GComponent.GetChildAt(0) as fgui.GGraph);
		this.m_dragArea = (this.GComponent.GetChildAt(1) as fgui.GGraph);
	}
}