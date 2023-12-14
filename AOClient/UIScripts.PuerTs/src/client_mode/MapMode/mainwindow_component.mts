import "csharp";
import "puerts";
import ElementComponent from "../../ui_base/element_component.mjs";
import UIElement from "../../ui_base/uielement.mjs";
import UI_MainWindow from "../../ui_scripts/auto_generates/Login/UI_MainWindow.mjs";
import UIRoot from "../../ui_scripts/uiroot.mjs";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import AOGame = CS.AO.AOGame;

export default class MainWindowComponent extends ElementComponent {

    public ui:UI_MainWindow;

    constructor(ui:UIElement) {
        super(ui);
        this.ui = ui as UI_MainWindow;
    }

    awake() {
        super.awake();
        console.log("MainComponent awake");
        this.ui.g_skillCureBtn.onClick.Add(this.onBtnClick);
    }

    async onBtnClick() {
        console.log("onBtnClick");
        var msg = new ET.C2M_SpellRequest();
        msg.SkillId = 1003;
        await ppromise(AO.AvatarCallTs.C2M_SpellRequest(msg));
    }
}