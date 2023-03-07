//FYI: https://github.com/Tencent/puerts/blob/master/doc/unity/manual.md

import { FairyEditor } from 'csharp';
import { genCode } from './GenCode_CSharp';

function onPublish(handler: FairyEditor.PublishHandler) {
    if (!handler.genCode) return;
    handler.genCode = false; //prevent default output

    console.log('Handling gen code in plugin');
    genCode(handler); //do it myself
}

function onDestroy() {
    //do cleanup here
}

export { onPublish, onDestroy };