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

export default class UIWindow extends UIElement {

    public window:fgui.Window;

    constructor(GObject: fgui.GObject) {
        super(GObject);
        this.window = new fgui.Window();
        this.window.contentPane = this.gobj.asCom;
    }

    showWindow(parent:fgui.GComponent){
        UIRoot.Windows.set(this.constructor.name, this);
        parent.ShowWindow(this.window);
        this.window.MakeFullScreen();
        this.onOpen();
    }

    hideWindow(){
        this.window.Hide();
        this.onClose();
    }

    dispose(){
        UIRoot.Windows.delete(typeof(this));
        this.components.clear();
        this.window.Dispose();
    }
}
