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
    loginWindow.showWindow();
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
    var loginTask = modeComp.Login();
    await ppromise(loginTask);

    // let msg = new ET.C2G_LoginGate();
    // msg.Key = BigInt(101);
    // let task = AO.ServerCall.C2G_LoginGate(msg);
    // await puer.$promise(task);

    // let msgEnter = new ET.C2G_EnterMap();
    // let taskEnter = AO.ServerCall.C2G_EnterMap(msgEnter);
    // await puer.$promise(taskEnter);

    // let sceneTask = CS.UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Map1");
}

function onLeave () {
    
}

export function register() {
    UIRoot.FuncMap.set("LoginMode_Enter", onEnter);
    UIRoot.FuncMap.set("LoginMode_Leave", onLeave);
}