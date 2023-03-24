import "csharp";
import "puerts";
import UIRoot from "../../ui_scripts/uiroot.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;
import Component = CS.UnityEngine.Component;
import GameObject = CS.UnityEngine.GameObject;
import UI_LoginWindow from "../../ui_scripts/auto_generates/Login/UI_LoginWindow.mjs";

function onEnter () {
    var pack = "Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var loginWindow = UI_LoginWindow.createInstance();
    loginWindow.showWindow(UIRoot.MiddUIView);
    loginWindow.g_loginBtn.onClick.Add(login);
    var modeComp = AOGame.ClientApp.GetComponentof(AO.LoginModeComponent);
    modeComp.AddDisposeAction(function () {
        loginWindow.dispose();
        AO.UIUtils.RemovePackage("Login");
        asset.Dispose();
    });
}

async function login() {
    var modeComp = AOGame.ClientApp.GetComponentof(AO.LoginModeComponent);
    var account = UI_LoginWindow.getInstance().g_accountInput.g_inputtext.text;
    var password = UI_LoginWindow.getInstance().g_passwordInput.g_inputtext.text;
    console.log("login " + account + "  " + password);
    if (account == "" || password == "") {
        return;
    }
    var loginTask = modeComp.Login(account, password);
    await ppromise(loginTask);
}

function onLeave () {
    
}

export function register() {
    UIRoot.FuncMap.set("LoginMode_Enter", onEnter);
    UIRoot.FuncMap.set("LoginMode_Leave", onLeave);
}