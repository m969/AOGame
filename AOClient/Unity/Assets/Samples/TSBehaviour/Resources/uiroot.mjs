import "csharp";
import "puerts";
var fgui = CS.FairyGUI;
var AO = CS.AO;
export default class UIRoot {
    constructor() {
        console.log("uiroot start.");
        UIRoot.inst = this;
        fgui.UIPackage.unloadBundleByFGUI = false;
        let groot = fgui.GRoot.inst;
        let asset = AO.UIUtils.LoadPackage("Common");
        var uirootObj = fgui.UIPackage.CreateObject("Common", "UIRoot");
        groot.AddChild(uirootObj);
        uirootObj.MakeFullScreen();
        uirootObj.Center();
        UIRoot.MiddUIView = uirootObj.asCom.GetChild("UIMidd").asCom;
        UIRoot.FrontUIView = uirootObj.asCom.GetChild("UIFront").asCom;
    }
    onUpdate() {
    }
    onDestroy() {
    }
}
UIRoot.FuncMap = new Map();
UIRoot.Windows = new Map();
