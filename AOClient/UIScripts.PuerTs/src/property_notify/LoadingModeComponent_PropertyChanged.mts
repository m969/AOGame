import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;
import UIWindow from "../ui_base/uiwindow.mjs";
import UI_LoadingWindow from "../ui_scripts/auto_generates/Loading/UI_LoadingWindow.mjs";

let funcs = new Array<Function>();
let func = (o:any) => {  };

// 属性变更通知：等级
func = function LoadingProgress_Changed (owner: AO.LoadingModeComponent) {
    //console.log("LoadingProgress_Changed " + owner.LoadingProgress);
    if (UI_LoadingWindow.getInstance() == null){
        return;
    }
    UI_LoadingWindow.getInstance().g_loadingProgressBar.value = owner.LoadingProgress;
}
funcs.push(func);

export function register() {
    funcs.forEach((f) => {
        UIRoot.FuncMap.set("LoadingModeComponent_" + f.name, f);
    });
}