import "csharp";
import "puerts";
import UI_MainWindow from "../../ui_scripts/ui_windows/Login/UI_MainWindow.mjs";
import LoginFactory from "../../ui_scripts/auto_generates/Login/LoginFactory.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
import MainWindowComponent from "./mainwindow_component.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

function onEnter () {
    var pack = "Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = LoginFactory.create_UI_MainWindow();
    // window.addComponent(MainWindowComponent);
    window.showWindow(UIRoot.MiddUIView);
    let modeComp = AOGame.ClientApp.GetComponent(ptypeof(AO.MapModeComponent)) as AO.MapModeComponent;
    modeComp.AddDisposeAction(function () {
        window.dispose();
        AO.UIUtils.RemovePackage("Login");
        asset.Dispose();
    });
}

function onLeave () {
    
}

export function register() {
    UIRoot.FuncMap.set("MapMode_Enter", onEnter);
    UIRoot.FuncMap.set("MapMode_Leave", onLeave);
}