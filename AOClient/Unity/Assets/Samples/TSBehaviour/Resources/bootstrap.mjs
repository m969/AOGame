// @ts-nocheck
// Object.defineProperty(globalThis, 'csharp', { value: require("csharp"), enumerable: true, configurable: false, writable: false });
// Object.defineProperty(globalThis, 'puerts', { value: require("puerts"), enumerable: true, configurable: false, writable: false });
import UIRoot from "./ui_scripts/uiroot.mjs";
// require("webapi.js");
// module.exports = require("bundle.js").default;
function init() {
    new UIRoot();
}
export function callback(type, arg) {
    if (type == "init") {
        init();
        return;
    }
    let func = UIRoot.FuncMap.get(type);
    func();
}
