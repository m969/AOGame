import "csharp";
import "puerts";
import UIRoot from "../../ui_scripts/uiroot.mjs";
var ptypeof = puer.$typeof;
var AO = CS.AO;
var AOGame = CS.AO.AOGame;
import UI_MainWindowFactory from "./UI_MainWindowFactory.mjs";
function onEnter() {
    var pack = "Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = UI_MainWindowFactory.createInstance();
    // window.addComponent(MainWindowComponent);
    window.showWindow(UIRoot.MiddUIView);
    let modeComp = AOGame.ClientApp.GetComponent(ptypeof(AO.MapModeComponent));
    modeComp.AddDisposeAction(function () {
        window.dispose();
        AO.UIUtils.RemovePackage("Login");
        asset.Dispose();
    });
}
function onLeave() {
}
export function register() {
    UIRoot.FuncMap.set("MapMode_Enter", onEnter);
    UIRoot.FuncMap.set("MapMode_Leave", onLeave);
}
