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
var fgui = CS.FairyGUI;
import { UI_MainWindow } from "./UI_MainWindow.mjs";
export default class UI_MainWindowFactory {
    static createInstance() {
        let obj = fgui.UIPackage.CreateObject("Login", "MainWindow");
        return new UI_MainWindow(obj);
    }
}
