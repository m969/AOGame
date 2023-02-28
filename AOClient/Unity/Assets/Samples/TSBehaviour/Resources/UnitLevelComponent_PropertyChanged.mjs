import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
let funcs = new Array();
let func = (o) => { };
// 属性变更通知：等级
func = function Level_Changed(owner) {
    console.log("Level_Changed " + owner.Level);
};
funcs.push(func);
export function register() {
    funcs.forEach((f) => {
        UIRoot.FuncMap.set("UnitLevelComponent_" + f.name, f);
    });
}
