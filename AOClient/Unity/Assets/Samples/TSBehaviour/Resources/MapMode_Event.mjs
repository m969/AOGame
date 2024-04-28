import "csharp";
import "puerts";
import LoginFactory from "../../ui_scripts/auto_generates/Login/LoginFactory.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
var ptypeof = puer.$typeof;
var AO = CS.AO;
var AOGame = CS.AO.AOGame;
function onEnter() {
    var pack = "Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = LoginFactory.create_UI_MainWindow();
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
