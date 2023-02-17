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
import UIRoot from "../../ui_scripts/uiroot.mjs";
var ppromise = puer.$promise;
var AO = CS.AO;
var AOGame = CS.AO.AOGame;
import UI_LoginWindow from "../../ui_scripts/auto_generates/Login/UI_LoginWindow.mjs";
function onEnter() {
    var pack = "Login";
    var asset = AO.UIUtils.LoadPackage(pack);
    var loginWindow = UI_LoginWindow.createInstance();
    loginWindow.showWindow();
    loginWindow.g_loginBtn.onClick.Add(login);
    let modeComp = AOGame.ClientApp.Get(AO.LoginModeComponent);
    modeComp.AddDisposeAction(function () {
        loginWindow.dispose();
        AO.UIUtils.RemovePackage("Login");
        asset.Dispose();
    });
}
function login() {
    return __awaiter(this, void 0, void 0, function* () {
        let modeComp = AOGame.ClientApp.Get(AO.LoginModeComponent);
        let loginTask = modeComp.Login();
        yield ppromise(loginTask);
        // let msg = new ET.C2G_LoginGate();
        // msg.Key = BigInt(101);
        // let task = AO.ServerCall.C2G_LoginGate(msg);
        // await puer.$promise(task);
        // let msgEnter = new ET.C2G_EnterMap();
        // let taskEnter = AO.ServerCall.C2G_EnterMap(msgEnter);
        // await puer.$promise(taskEnter);
        // let sceneTask = CS.UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Map1");
    });
}
function onLeave() {
}
export function register() {
    UIRoot.FuncMap.set("LoginMode_Enter", onEnter);
    UIRoot.FuncMap.set("LoginMode_Leave", onLeave);
}
