import "csharp";
import "puerts";
var fgui = CS.FairyGUI;
export default class UIRoot {
    constructor() {
        this.speed = 0;
        console.log("uiroot start.");
        UIRoot.inst = this;
        fgui.UIPackage.unloadBundleByFGUI = false;
        let groot = fgui.GRoot.inst;
    }
    onUpdate() {
    }
    onDestroy() {
    }
}
UIRoot.FuncMap = new Map();
