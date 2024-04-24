import "csharp";
import "puerts";
import ptypeof = puer.$typeof;
import ppromise = puer.$promise;
import fgui = CS.FairyGUI;
import ET = CS.ET;
import AO = CS.AO;
import UI_MainWindowObject from "../../ui_scripts/auto_generates/Login/UI_MainWindowObject.mjs";

class UI_MainWindow extends UI_MainWindowObject {
    constructor(GObject: fgui.GObject) {
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

    async onBtnClick() {
        console.log("onBtnClick");
        var msg = new ET.C2M_SpellRequest();
        msg.SkillId = 1003;
        await ppromise(AO.AvatarCallTs.C2M_SpellRequest(msg));
    }
}

export { UI_MainWindow };