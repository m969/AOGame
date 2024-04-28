/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

import "csharp";
import "puerts";
import fgui = CS.FairyGUI;
import UI_LobbyWindow from "../../ui_windows/Login/UI_LobbyWindow.mjs";
import UI_LoginWindow from "../../ui_windows/Login/UI_LoginWindow.mjs";
import UI_MainWindow from "../../ui_windows/Login/UI_MainWindow.mjs";

export default class LoginFactory {
	public static create_UI_LobbyWindow():UI_LobbyWindow {
		let obj = fgui.UIPackage.CreateObject("Login", "LobbyWindow") as fgui.GComponent;
		return new UI_LobbyWindow(obj);
	}
	public static create_UI_LoginWindow():UI_LoginWindow {
		let obj = fgui.UIPackage.CreateObject("Login", "LoginWindow") as fgui.GComponent;
		return new UI_LoginWindow(obj);
	}
	public static create_UI_MainWindow():UI_MainWindow {
		let obj = fgui.UIPackage.CreateObject("Login", "MainWindow") as fgui.GComponent;
		return new UI_MainWindow(obj);
	}
}