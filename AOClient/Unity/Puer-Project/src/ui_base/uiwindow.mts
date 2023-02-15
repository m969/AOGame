import "csharp";
import "puerts";
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
        this.window.contentPane = this.GObject.asCom;
    }

    showWindow(){
        this.window.Show();
        this.window.MakeFullScreen();
    }

    dispose(){
        this.components.clear();
        this.window.Dispose();
    }
}
