import "csharp";
import "puerts";
import UIRoot from "../ui_scripts/uiroot.mjs";
import UIElement from "./uielement.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;
import UIWindow from "./uiwindow.mjs";

export default class UIFunctions {

    open(window:UIWindow){
        window.showWindow(UIRoot.MiddUIView);
    }

    close(window:UIWindow){
        window.hideWindow();
    }

    destroy(window:UIWindow){
        window.dispose();
    }
}
