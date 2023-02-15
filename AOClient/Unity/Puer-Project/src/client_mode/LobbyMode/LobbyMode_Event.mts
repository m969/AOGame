import "csharp";
import "puerts";
import UI_LobbyWindow from "../../ui_scripts/auto_generates/Login/UI_LobbyWindow.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

function onEnter () {
    var pack = "Assets/Bundles/UIRes/Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = UI_LobbyWindow.createInstance();
    window.showWindow();
    window.g_enterBtn.onClick.Add(enterMap);
    let modeComp = AOGame.ClientApp.GetComponent(ptypeof(AO.LobbyModeComponent)) as AO.LobbyModeComponent;
    modeComp.AddDisposeAction(function () {
        window.dispose();
        AO.UIUtils.RemovePackage("Login");
        asset.Dispose();
    });
}

async function enterMap() {
    let modeComp = AOGame.ClientApp.GetComponent(ptypeof(AO.LobbyModeComponent)) as AO.LobbyModeComponent;
    let task = modeComp.EnterMap();
    await ppromise(task);
}

function onLeave () {
    
}

export function register() {
    UIRoot.FuncMap.set("LobbyMode_Enter", onEnter);
    UIRoot.FuncMap.set("LobbyMode_Leave", onLeave);
}