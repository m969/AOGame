/** This is an automatically generated class by FairyGUI. Please do not modify it. **/


import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_UIRoot  extends UIElement  {

	public g_UIHide:fgui.GComponent;
	public g_UIBack:fgui.GComponent;
	public g_UIMidd:fgui.GComponent;
	public g_UIFront:fgui.GComponent;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://bch3lpzzj3ot8";

	public static createInstance():UI_UIRoot {
		let GObject = (fgui.UIPackage.CreateObject("Common", "UIRoot"));
		var ui = new UI_UIRoot(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_UIHide = (this.GComponent.GetChildAt(0) as fgui.GComponent);
		this.g_UIBack = (this.GComponent.GetChildAt(1) as fgui.GComponent);
		this.g_UIMidd = (this.GComponent.GetChildAt(2) as fgui.GComponent);
		this.g_UIFront = (this.GComponent.GetChildAt(3) as fgui.GComponent);
	}
}