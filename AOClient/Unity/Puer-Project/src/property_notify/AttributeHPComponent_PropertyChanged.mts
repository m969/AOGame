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

// 属性变更通知：生命值
func = function Attribute_HP_Changed (owner: AO.AttributeHPComponent) {
    console.log("Attribute_HP_Changed " + owner.Attribute_HP);
}
funcs.push(func);

// 属性变更通知：生命值
func = function Available_HP_Changed (owner: AO.AttributeHPComponent) {
    console.log("Available_HP_Changed " + owner.Available_HP);
}
funcs.push(func);

export function register() {
    funcs.forEach((f) => {
        UIRoot.FuncMap.set("AttributeHPComponent_" + f.name, f);
    });
}