/** This is an automatically generated class by FairyGUI. Please do not modify it. **/
import "csharp";
import "puerts";
var fgui = CS.FairyGUI;
import UI_LobbyWindow from "../../ui_windows/Login/UI_LobbyWindow.mjs";
import UI_LoginWindow from "../../ui_windows/Login/UI_LoginWindow.mjs";
import UI_MainWindow from "../../ui_windows/Login/UI_MainWindow.mjs";
export default class LoginFactory {
    static create_UI_LobbyWindow() {
        let obj = fgui.UIPackage.CreateObject("Login", "LobbyWindow");
        return new UI_LobbyWindow(obj);
    }
    static create_UI_LoginWindow() {
        let obj = fgui.UIPackage.CreateObject("Login", "LoginWindow");
        return new UI_LoginWindow(obj);
    }
    static create_UI_MainWindow() {
        let obj = fgui.UIPackage.CreateObject("Login", "MainWindow");
        return new UI_MainWindow(obj);
    }
}
