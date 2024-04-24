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
import "csharp";
import "puerts";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import { UI_MainWindow } from "./UI_MainWindow.mjs";

export default class UI_MainWindowFactory {
    static createInstance():UI_MainWindow{
        let obj = fgui.UIPackage.CreateObject("Login", "MainWindow") as fgui.GComponent;
        return new UI_MainWindow(obj);
    }
}
