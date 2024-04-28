import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
export default class UIFunctions {
    static getWindow(TClass) {
        return UIRoot.Windows.get(TClass.name);
    }
    static open(window) {
        window.showWindow(UIRoot.MiddUIView);
    }
    static close(window) {
        window.hideWindow();
    }
    static destroy(window) {
        window.dispose();
    }
}
