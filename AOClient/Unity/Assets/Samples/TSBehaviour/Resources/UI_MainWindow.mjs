var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import "csharp";
import "puerts";
var ppromise = puer.$promise;
var ET = CS.ET;
var AO = CS.AO;
import UI_MainWindowObject from "../../auto_generates/Login/UI_MainWindowObject.mjs";
export default class UI_MainWindow extends UI_MainWindowObject {
    constructor(GObject) {
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
    onBtnClick() {
        return __awaiter(this, void 0, void 0, function* () {
            console.log("onBtnClick");
            var msg = new ET.C2M_SpellRequest();
            msg.SkillId = 1003;
            yield ppromise(AO.AvatarCallTs.C2M_SpellRequest(msg));
        });
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
