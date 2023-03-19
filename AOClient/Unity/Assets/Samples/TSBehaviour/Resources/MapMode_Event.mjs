import "csharp";
import "puerts";
import UI_MainWindow from "../../ui_scripts/auto_generates/Login/UI_MainWindow.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
import MainComponent from "./main_component.mjs";
var ptypeof = puer.$typeof;
var AO = CS.AO;
var AOGame = CS.AO.AOGame;
function onEnter() {
    var pack = "Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = UI_MainWindow.createInstance();
    window.addComponent(MainComponent);
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
