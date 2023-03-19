import "csharp";
import "puerts";
import ElementComponent from "../../ui_base/element_component.mjs";
export default class MainComponent extends ElementComponent {
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
        console.log("onBtnClick");
    }
}
