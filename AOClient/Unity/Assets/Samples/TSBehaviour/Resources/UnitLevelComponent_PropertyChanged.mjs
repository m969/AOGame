import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
// 属性变更通知：等级
function Level_Changed(owner) {
}
export function register() {
    UIRoot.FuncMap.set("UnitLevelComponent_Level_Changed", Level_Changed);
}
