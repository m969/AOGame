import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
// 属性变更通知：等级
function Level_Changed(avatar) {
}
export function register() {
    UIRoot.FuncMap.set("Avatar_Level_Changed", Level_Changed);
}
