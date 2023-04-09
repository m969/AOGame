import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
let funcs = new Array();
let func = (o) => { };
// 属性变更通知：生命值
func = function AttributeValue_Changed(owner) {
    console.log("AttributeValue_Changed1 " + owner.AttributeValue);
};
funcs.push(func);
// 属性变更通知：生命值
func = function AvailableValue_Changed(owner) {
    console.log("AvailableValue_Changed " + owner.AvailableValue);
};
funcs.push(func);
export function register() {
    funcs.forEach((f) => {
        UIRoot.FuncMap.set("AttributeHPComponent_" + f.name, f);
    });
}
