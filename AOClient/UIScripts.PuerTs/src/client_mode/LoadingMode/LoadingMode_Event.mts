import "csharp";
import "puerts";
import UI_LoadingWindow from "../../ui_scripts/auto_generates/Loading/UI_LoadingWindow.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

function onEnter () {
    var pack = "Loading";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = UI_LoadingWindow.createInstance();
    window.showWindow(UIRoot.FrontUIView);
    window.g_loadingProgressBar.value = 0;
    let modeComp = AOGame.ClientApp.GetComponentof(AO.LoadingModeComponent);
    modeComp.AddDisposeAction(function () {
        window.dispose();
        AO.UIUtils.RemovePackage(pack);
        asset.Dispose();
    });
}

function onLeave () {
    
}

export function register() {
    UIRoot.FuncMap.set("LoadingMode_Enter", onEnter);
    UIRoot.FuncMap.set("LoadingMode_Leave", onLeave);
}