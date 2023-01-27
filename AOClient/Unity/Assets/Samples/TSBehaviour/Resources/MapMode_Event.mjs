import "csharp";
import "puerts";
import UI_MainWindow from "../../ui_scripts/Login/UI_MainWindow.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
var ptypeof = puer.$typeof;
var fgui = CS.FairyGUI;
var AO = CS.AO;
var AOGame = CS.AO.AOGame;
function onEnter() {
    var pack = "Assets/Bundles/UIRes/Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = UI_MainWindow.createInstance();
    let win = new fgui.Window();
    win.contentPane = window.GComponent;
    win.Show();
    win.MakeFullScreen();
    let modeComp = AOGame.ClientApp.GetComponent(ptypeof(AO.MapModeComponent));
    modeComp.AddDisposeAction(function () {
        win.Dispose();
        AO.UIUtils.RemovePackage("Login");
        asset.Release();
    });
}
function onLeave() {
}
export function register() {
    UIRoot.FuncMap.set("MapMode_Enter", onEnter);
    UIRoot.FuncMap.set("MapMode_Leave", onLeave);
}
