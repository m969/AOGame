import "csharp";
import "puerts";
import UI_LobbyWindow from "../../ui_scripts/ui_windows/Login/UI_LobbyWindow.mjs";
import LoginFactory from "../../ui_scripts/auto_generates/Login/LoginFactory.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

function onEnter () {
    var pack = "Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = LoginFactory.create_UI_LobbyWindow();
    window.showWindow(UIRoot.MiddUIView);
    window.g_enterBtn.onClick.Add(enterMap);
    let modeComp = AOGame.ClientApp.GetComponent(ptypeof(AO.LobbyModeComponent)) as AO.LobbyModeComponent;
    modeComp.AddDisposeAction(function () {
        window.dispose();
        AO.UIUtils.RemovePackage("Login");
        asset.Dispose();
    });
}

async function enterMap() {
    AOGame.ClientApp.AddComponent(ptypeof(AO.LoadingModeComponent));
    let modeComp = AOGame.ClientApp.GetComponentof(AO.LobbyModeComponent);
    let task = modeComp.EnterMap();
    await ppromise(task);
}

function onLeave () {
    
}

export function register() {
    UIRoot.FuncMap.set("LobbyMode_Enter", onEnter);
    UIRoot.FuncMap.set("LobbyMode_Leave", onLeave);
}