/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_WindowFrame  extends UIElement  {

	public g_contentArea:fgui.GGraph;
	public g_dragArea:fgui.GGraph;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://hlimh2ngm78f3";

	public static createInstance():UI_WindowFrame {
		let GObject = (fgui.UIPackage.CreateObject("Login", "WindowFrame"));
		var ui = new UI_WindowFrame(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_contentArea = (this.GComponent.GetChildAt(0) as fgui.GGraph);
		this.g_dragArea = (this.GComponent.GetChildAt(1) as fgui.GGraph);
	}
}