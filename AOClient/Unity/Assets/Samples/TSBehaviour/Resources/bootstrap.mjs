// @ts-nocheck
// Object.defineProperty(globalThis, 'csharp', { value: require("csharp"), enumerable: true, configurable: false, writable: false });
// Object.defineProperty(globalThis, 'puerts', { value: require("puerts"), enumerable: true, configurable: false, writable: false });
import "csharp";
import "puerts";
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
    if (arg == null){
        func();
    }
    else{
        func(arg);
    }
}
CS.ET.Entity.prototype.GetComponentof = function () {
    return this.GetComponent(puer.$typeof(arguments[0]));
};
