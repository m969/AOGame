'use strict';
Object.defineProperty(exports, '__esModule', { value: true });
exports.onDestroy = exports.onPublish = void 0;
const GenCode_CSharp_1 = require('./GenCode_TS');
function onPublish(handler) {
    if (!handler.genCode)
        return;
    handler.genCode = false; //prevent default output
    console.log('Handling gen code in plugin');
    GenCode_CSharp_1.genCode(handler); //do it myself
}
exports.onPublish = onPublish;
function onDestroy() {
    //do cleanup here
}
exports.onDestroy = onDestroy;