/** This is an automatically generated class by FairyGUI. Please do not modify it. **/


import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_CommonWindowFrame  extends UIElement  {

	public g_c1:fgui.Controller;
	public g_contentArea:fgui.GGraph;
	public g_dragArea:fgui.GGraph;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://bch3lpzzm78f3";

	public static createInstance():UI_CommonWindowFrame {
		let GObject = (fgui.UIPackage.CreateObject("Common", "CommonWindowFrame"));
		var ui = new UI_CommonWindowFrame(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_c1 = this.GComponent.GetControllerAt(0);
		this.g_contentArea = (this.GComponent.GetChildAt(0) as fgui.GGraph);
		this.g_dragArea = (this.GComponent.GetChildAt(1) as fgui.GGraph);
	}
}