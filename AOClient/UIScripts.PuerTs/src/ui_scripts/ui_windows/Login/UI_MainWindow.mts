import "csharp";
import "puerts";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import UI_MainWindowObject from "../../auto_generates/Login/UI_MainWindowObject.mjs";

export default class UI_MainWindow extends UI_MainWindowObject {
    constructor(GObject: fgui.GObject) {
        super(GObject);

        this.g_skillCureBtn.onClick.Add(this.onBtnClick);
    }

    onOpen() {
        super.onOpen();
        console.log("UI_MainWindow onOpen");
    }

    onClose() {
        super.onClose();
        console.log("UI_MainWindow onClose");
    }

    async onBtnClick() {
        console.log("onBtnClick");
        var msg = new ET.C2M_SpellRequest();
        msg.SkillId = 1003;
        await ppromise(AO.AvatarCallTs.C2M_SpellRequest(msg));
    }
}

// // declare namespace CS {
// //     class UI_MainWindow {
// //         public static createInstance(): UI_MainWindow;
// //         public static getInstance(): UI_MainWindow;
// //     }
// // }

// declare namespace UI_MainWindow {
//     public static createInstance(): UI_MainWindow;
//     public static getInstance(): UI_MainWindow;
// }

// CS.UI_MainWindow.createInstance = function() {
//     let GObject = (fgui.UIPackage.CreateObject("Login", "MainWindow"));
//     var ui = new CS.UI_MainWindow(GObject);
//     return ui;
// }

// CS.UI_MainWindow.getInstance = function() {
//     return AO.UIRoot.Windows.get("UI_MainWindow") as AO.UI_MainWindow;
// }

// export { UI_MainWindow };