import "csharp";
import "puerts";
import UI_LoadingWindow from "../../ui_scripts/auto_generates/Loading/UI_LoadingWindow.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
var AO = CS.AO;
var AOGame = CS.AO.AOGame;
function onEnter() {
    var pack = "Loading";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = UI_LoadingWindow.createInstance();
    window.showWindow(UIRoot.FrontUIView);
    let modeComp = AOGame.ClientApp.GetComponentof(AO.LoadingModeComponent);
    window.g_loadingProgressBar.value = modeComp.LoadingProgress;
    modeComp.AddDisposeAction(function () {
        window.dispose();
        AO.UIUtils.RemovePackage(pack);
        asset.Dispose();
    });
}
function onLeave() {
}
export function register() {
    UIRoot.FuncMap.set("LoadingMode_Enter", onEnter);
    UIRoot.FuncMap.set("LoadingMode_Leave", onLeave);
}
