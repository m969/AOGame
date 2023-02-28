import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

let funcs = new Array<Function>();
let func = (o:any) => {  };

// 属性变更通知：等级
func = function Level_Changed (owner: AO.UnitLevelComponent) {
    console.log("Level_Changed " + owner.Level);
}
funcs.push(func);

export function register() {
    funcs.forEach((f) => {
        UIRoot.FuncMap.set("UnitLevelComponent_" + f.name, f);
    });
}