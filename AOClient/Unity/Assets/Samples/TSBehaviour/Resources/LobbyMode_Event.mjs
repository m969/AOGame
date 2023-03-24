var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import "csharp";
import "puerts";
import UI_LobbyWindow from "../../ui_scripts/auto_generates/Login/UI_LobbyWindow.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
var ptypeof = puer.$typeof;
var ppromise = puer.$promise;
var AO = CS.AO;
var AOGame = CS.AO.AOGame;
function onEnter() {
    var pack = "Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var window = UI_LobbyWindow.createInstance();
    window.showWindow(UIRoot.MiddUIView);
    window.g_enterBtn.onClick.Add(enterMap);
    let modeComp = AOGame.ClientApp.GetComponent(ptypeof(AO.LobbyModeComponent));
    modeComp.AddDisposeAction(function () {
        window.dispose();
        AO.UIUtils.RemovePackage("Login");
        asset.Dispose();
    });
}
function enterMap() {
    return __awaiter(this, void 0, void 0, function* () {
        AOGame.ClientApp.AddComponent(ptypeof(AO.LoadingModeComponent));
        let modeComp = AOGame.ClientApp.GetComponentof(AO.LobbyModeComponent);
        let task = modeComp.EnterMap();
        yield ppromise(task);
    });
}
function onLeave() {
}
export function register() {
    UIRoot.FuncMap.set("LobbyMode_Enter", onEnter);
    UIRoot.FuncMap.set("LobbyMode_Leave", onLeave);
}
