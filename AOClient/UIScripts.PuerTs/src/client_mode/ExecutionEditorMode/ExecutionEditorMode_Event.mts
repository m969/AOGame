import "csharp";
import "puerts";
import UIRoot from "../../ui_scripts/uiroot.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

function onEnter () {

}

function onLeave () {
    
}

export function register() {
    UIRoot.FuncMap.set("ExecutionEditorMode_Enter", onEnter);
    UIRoot.FuncMap.set("ExecutionEditorMode_Leave", onLeave);
}