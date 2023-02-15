import "csharp";
import "puerts";
import UIRoot from "../../ui_scripts/uiroot.mjs";
function onEnter() {
}
function onLeave() {
}
export function register() {
    UIRoot.FuncMap.set("ExecutionEditorMode_Enter", onEnter);
    UIRoot.FuncMap.set("ExecutionEditorMode_Leave", onLeave);
}
