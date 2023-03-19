/** This is an automatically generated class by FairyGUI. Please do not modify it. **/


import "csharp";
import fgui = CS.FairyGUI;
import UIWindow from "../../../ui_base/uiwindow.mjs";
import UIElement from "../../../ui_base/uielement.mjs";
export default class UI_HealthBar  extends UIElement  {

	public g_tipsText:fgui.GTextField;
	public g_t0:fgui.Transition;
	public GComponent:fgui.GComponent;
	public static URL:string = "ui://bch3lpzzdpkf7";

	public static createInstance():UI_HealthBar {
		let GObject = (fgui.UIPackage.CreateObject("Common", "HealthBar"));
		var ui = new UI_HealthBar(GObject);
		return ui;
	}

	constructor(GObject: fgui.GObject) {
		super(GObject);
		this.GComponent = GObject.asCom;
		this.g_tipsText = (this.GComponent.GetChildAt(2) as fgui.GTextField);
		this.g_t0 = this.GComponent.GetTransitionAt(0);
	}
}