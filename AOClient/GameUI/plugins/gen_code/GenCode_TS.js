"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.genCode = void 0;
const csharp_1 = require("csharp");
const CodeWriter_1 = require("./CodeWriter");
function genCode(handler) {
    let settings = handler.project.GetSettings("Publish").codeGeneration;
    let codePkgName = handler.ToFilename(handler.pkg.name); //convert chinese to pinyin, remove special chars etc.
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    let namespaceName = codePkgName;
    let ns = "fgui";
    let isThree = handler.project.type == csharp_1.FairyEditor.ProjectType.ThreeJS;
    if (settings.packageName)
        namespaceName = settings.packageName + '.' + namespaceName;
    //CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    let classes = handler.CollectClasses(settings.ignoreNoname, settings.ignoreNoname, ns);
    handler.SetupCodeFolder(exportCodePath, "ts"); //check if target folder exists, and delete old files
    let getMemberByName = settings.getMemberByName;
    let classCnt = classes.Count;
    let writer = new CodeWriter_1.default({ blockFromNewLine: false, usingTabs: true });
    for (let i = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let members = classInfo.members;
        let references = classInfo.references;
        writer.reset();
        let refCount = references.Count;
        if (refCount > 0) {
            for (let j = 0; j < refCount; j++) {
                let ref = references.get_Item(j);
                writer.writeln('import %s from "./%s.mjs";', ref, ref);
            }
            writer.writeln();
        }
        if (isThree) {
            writer.writeln('import * as fgui from "fairygui-three";');
            if (refCount == 0)
                writer.writeln();
        }
        writer.writeln('import "csharp";');
        writer.writeln('import fgui = CS.FairyGUI;');
        // writer.writeln('import {$ref, $unref, $generic, $promise, $typeof} from \'puerts\'');
        writer.writeln('export default class %s', classInfo.className);
        writer.startBlock();
        writer.writeln();
        let memberCnt = members.Count;
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            writer.writeln('public %s:%s;', memberInfo.varName, memberInfo.type);
            // if (memberInfo.type.indexOf("fgui.") != -1){
            //     writer.writeln('public %s:%s;', memberInfo.varName, memberInfo.type);
            // }
            // else{
            //     writer.writeln('public %s:%s;', memberInfo.varName, memberInfo.type);
            // }
        }
        writer.writeln('public GObject:%s.GObject;', ns);
        writer.writeln('public GComponent:%s.GComponent;', ns);
        writer.writeln('public static URL:string = "ui://%s%s";', handler.pkg.id, classInfo.resId);
        writer.writeln();
        writer.writeln('public static createInstance():%s', classInfo.className);
        writer.startBlock();
        writer.writeln('let GObject = (%s.UIPackage.CreateObject("%s", "%s"));', ns, handler.pkg.name, classInfo.resName);
        writer.writeln('var ui = new %s(GObject);', classInfo.className);
        writer.writeln('return ui;');
        writer.endBlock();
        writer.writeln();
        writer.writeln('constructor(GObject: fgui.GObject)');
        writer.startBlock();
        writer.writeln('this.GObject = GObject;');
        writer.writeln('this.GComponent = GObject.asCom;');
        for (let j = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            if (memberInfo.group == 0) {
                if (getMemberByName){
                    if (memberInfo.type.indexOf("fgui.") != -1){
                        writer.writeln('this.%s = (this.GComponent.GetChild("%s") as %s);', memberInfo.varName, memberInfo.name, memberInfo.type);
                    }
                    else{
                        writer.writeln('this.%s = new %s(this.GComponent.GetChild(%s));', memberInfo.varName, memberInfo.type, memberInfo.name);
                    }
                }
                else{
                    if (memberInfo.type.indexOf("fgui.") != -1){
                        writer.writeln('this.%s = (this.GComponent.GetChildAt(%s) as %s);', memberInfo.varName, memberInfo.index, memberInfo.type);
                    }
                    else{
                        writer.writeln('this.%s = new %s(this.GComponent.GetChildAt(%s));', memberInfo.varName, memberInfo.type, memberInfo.index);
                    }
                }
            }
            else if (memberInfo.group == 1) {
                if (getMemberByName)
                    writer.writeln('this.%s = this.GComponent.GetController("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('this.%s = this.GComponent.GetControllerAt(%s);', memberInfo.varName, memberInfo.index);
            }
            else {
                if (getMemberByName)
                    writer.writeln('this.%s = this.GComponent.GetTransition("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('this.%s = this.GComponent.GetTransitionAt(%s);', memberInfo.varName, memberInfo.index);
            }
        }
        writer.endBlock();
        writer.endBlock(); //class
        writer.save(exportCodePath + '/' + classInfo.className + '.mts');
    }
    writer.reset();
    let binderName = codePkgName + 'Binder';
    for (let i = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        writer.writeln('import %s from "./%s.mjs";', classInfo.className, classInfo.className);
    }
    if (isThree) {
        writer.writeln('import * as fgui from "fairygui-three";');
        writer.writeln();
    }
    writer.writeln();
    writer.writeln('export default class %s', binderName);
    writer.startBlock();
    writer.writeln('public static bindAll():void');
    writer.startBlock();
    for (let i = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        writer.writeln('//%s.UIObjectFactory.setExtension(%s.URL, %s);', ns, classInfo.className, classInfo.className);
    }
    writer.endBlock(); //bindall
    writer.endBlock(); //class
    writer.save(exportCodePath + '/' + binderName + '.mts');
}
exports.genCode = genCode;
