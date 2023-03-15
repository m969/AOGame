import "csharp";
import "puerts";
import UIElement from "../ui_base/uielement.mjs";
import UIWindow from "../ui_base/uiwindow.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

export default class UIRoot {
    public static FuncMap:Map<string, Function> = new Map();
    public static Windows:Map<string, UIWindow> = new Map();
    public static inst:UIRoot;
    public speed: number = 0;

    constructor() {
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
