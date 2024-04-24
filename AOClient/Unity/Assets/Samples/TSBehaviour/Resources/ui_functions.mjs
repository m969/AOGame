import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
export default class UIFunctions {
    open(window) {
        window.showWindow(UIRoot.MiddUIView);
    }
    close(window) {
        window.hideWindow();
    }
    destroy(window) {
        window.dispose();
    }
}
