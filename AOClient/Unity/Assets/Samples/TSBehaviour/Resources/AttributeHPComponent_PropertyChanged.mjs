import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
let funcs = new Array();
let func = (o) => { };
// 属性变更通知：生命值
func = function Attribute_HP_Changed(owner) {
    console.log("Attribute_HP_Changed " + owner.Attribute_HP);
};
funcs.push(func);
// 属性变更通知：生命值
func = function Available_HP_Changed(owner) {
    console.log("Available_HP_Changed " + owner.Available_HP);
};
funcs.push(func);
export function register() {
    funcs.forEach((f) => {
        UIRoot.FuncMap.set("AttributeHPComponent_" + f.name, f);
    });
}
