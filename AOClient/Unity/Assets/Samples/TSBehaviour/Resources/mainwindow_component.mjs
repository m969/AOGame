var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import "csharp";
import "puerts";
import ElementComponent from "../../ui_base/element_component.mjs";
var ppromise = puer.$promise;
var ET = CS.ET;
var AO = CS.AO;
export default class MainWindowComponent extends ElementComponent {
    constructor(ui) {
        super(ui);
        this.ui = ui;
    }
    awake() {
        super.awake();
        console.log("MainComponent awake");
        this.ui.g_skillCureBtn.onClick.Add(this.onBtnClick);
    }
    onBtnClick() {
        return __awaiter(this, void 0, void 0, function* () {
            console.log("onBtnClick");
            var msg = new ET.C2M_SpellRequest();
            msg.SkillId = 1003;
            yield ppromise(AO.AvatarCallTs.C2M_SpellRequest(msg));
        });
    }
}
