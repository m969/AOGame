// import * as CS from "csharp";
// import { $generic, $typeof } from "puerts";
export {};
// import CSObject = CS.System.Object;
// import CSArray = CS.System.Array;
// import CSArray$1 = CS.System.Array$1;
// import CSList$1 = CS.System.Collections.Generic.List$1;
// import CSDictionary$2 = CS.System.Collections.Generic.Dictionary$2;
// import CSIEnumerator = CS.System.Collections.IEnumerator;
// import CSIEnumerator$1 = CS.System.Collections.Generic.IEnumerator$1;
// type Array$1Iterator<T> = {
//     /**
//      * 遍历Array$1对象
//      * @param callbackfn 
//      */
//     forEach(callbackfn: (v: T, index: number) => boolean | void): void;
//     /**
//      * 实现for迭代器接口
//      */
//     [Symbol.iterator](): {
//         next(): {
//             value: T,
//             done: boolean
//         };
//     };
// }
// function defineArrayIterator(prototype: any) {
//     prototype["forEach"] = function (callbackfn: (v: any, k: any) => void | boolean) {
//         let length = this.Length;
//         for (let i = 0; i < length; i++) {
//             if (callbackfn(this.GetValue(i), i) === false)
//                 break;
//         }
//     }
//     prototype[Symbol.iterator] = function* () {
//         let length = this.Length;
//         for (let i = 0; i < length; i++) {
//             yield this.GetValue(i);
//         }
//     }
// }
// type List$1Iterator<T> = {
//     /**
//      * 遍历List$1对象
//      * @param callbackfn 
//      */
//     forEach(callbackfn: (v: T, index: number) => boolean | void): void;
//     /**
//      * 实现for迭代器接口
//      */
//     [Symbol.iterator](): {
//         next(): {
//             value: T,
//             done: boolean
//         };
//     };
// }
// function defineListIterator(prototype: any) {
//     prototype["forEach"] = function (callbackfn: (v: any, k: any) => void | boolean) {
//         let iterator = this.GetEnumerator(), index = 0;
//         while (iterator.MoveNext()) {
//             if (callbackfn(iterator.Current, index++) === false)
//                 break;
//         }
//     }
//     prototype[Symbol.iterator] = function* () {
//         let iterator = this.GetEnumerator();
//         while (iterator.MoveNext()) {
//             yield iterator.Current;
//         }
//     }
// }
// type Dictionary$2Iterator<TKey, TValue> = {
//     /**
//      * 遍历Dictionary$2对象
//      * @param callbackfn 
//      */
//     forEach(callbackfn: (v: TValue, k?: TKey) => boolean | void): void;
//     /**
//      * Key集合
//      */
//     getKeys(): TKey[];
//     /**
//      * Value集合
//      */
//     getValues(): TValue[];
//     /**
//      * 实现for迭代器接口
//      */
//     [Symbol.iterator](): {
//         next(): {
//             value: [key: TKey, value: TValue],
//             done: boolean
//         };
//     };
// };
// function defineDictionaryIterator(prototype: any) {
//     prototype["forEach"] = function (callbackfn: (v: any, k: any) => void | boolean) {
//         let iterator = this.Keys.GetEnumerator();
//         while (iterator.MoveNext()) {
//             let key = iterator.Current;
//             if (callbackfn(this.get_Item(key), key) === false)
//                 break;
//         }
//     }
//     prototype["getKeys"] = function () {
//         let result = new Array<string>();
//         let iterator = this.Keys.GetEnumerator();
//         while (iterator.MoveNext()) {
//             result.push(iterator.Current);
//         }
//         return result;
//     }
//     prototype["getValues"] = function () {
//         let result = new Array<string>();
//         let iterator = this.Values.GetEnumerator();
//         while (iterator.MoveNext()) {
//             result.push(iterator.Current);
//         }
//         return result;
//     }
//     prototype[Symbol.iterator] = function* () {
//         let iterator = this.Keys.GetEnumerator();
//         while (iterator.MoveNext()) {
//             let key = iterator.Current;
//             yield [key, this.get_Item(key)];
//         }
//     }
// }
// type IEnumerator$1Iterator<T> = {
//     /**
//      * 遍历List$1对象
//      * @param callbackfn 
//      */
//     forEach(callbackfn: (v: T, index: number) => boolean | void): void;
//     /**
//      * 实现for迭代器接口
//      */
//     [Symbol.iterator](): {
//         next(): {
//             value: T,
//             done: boolean
//         };
//     };
// }
// type IEnumeratorType<T> =
//     { GetEnumerator(): CSIEnumerator$1<T>; } |
//     { GetEnumerator(): CSIEnumerator; };
// function defineIEnumeratorIterator(prototype: any) {
//     prototype["forEach"] = function (callbackfn: (v: any, k: any) => void | boolean) {
//         let iterator = this.GetEnumerator(), index = 0;
//         while (iterator.MoveNext()) {
//             if (callbackfn(iterator.Current, index++) === false)
//                 break;
//         }
//     }
//     prototype[Symbol.iterator] = function* () {
//         let iterator = this.GetEnumerator();
//         while (iterator.MoveNext()) {
//             yield iterator.Current;
//         }
//     }
// }
// const ITERATOR_DEFINE = Symbol(`ITERATOR_DEFINE`);
// /**迭代System.Array.Array$1对象
//  * @example
//  * ```
//  * let obj: System.Array$1<number>;
//  * let objIterator = iterator(obj);
//  * let jsArray = [...objIterator];              //number[]
//  * ```
//  */
// export function iterator<T = any>(instance: CSArray$1<T>): CSArray$1<T> & Array$1Iterator<T>;
// export function iterator(instance: CSArray): CSArray & Array$1Iterator<any>;                    //排在Array$1<T>参数声明之后, 降低Array参数匹配优先级
// /**迭代System.Collections.Generic.List$1对象
//  * @example
//  * ```
//  * let obj: System.Collections.Generic.List$1<number>;
//  * let objIterator = iterator(obj);
//  * let jsArray = [...objIterator];              //number[]
//  * ```
//  */
// export function iterator<T = any>(instance: CSList$1<T>): CSList$1<T> & List$1Iterator<T>;
// /**迭代System.Collections.Generic.Dictionary$2对象
//  * @example
//  * ``` 
//  * let obj: System.Collections.Generic.Dictionary$2<number, string>;
//  * let objIterator = iterator(obj);
//  * let keys = objIterator.getKeys();            //number[]
//  * let values = objIterator.getValues();        //string[]
//  * let keyValuePairs = [...objIterator];        //Array<[key: number, value: string ]>
//  * ```
//  */
// export function iterator<TKey = any, TValue = any>(instance: CSDictionary$2<TKey, TValue>): CSDictionary$2<TKey, TValue> & Dictionary$2Iterator<TKey, TValue>;
// /**迭代System.Collections.IEnumerable接口实现
//  * @example
//  * ```
//  * let obj: CS.System.Collections.Generic.HashSet$1<number>;
//  * let objIterator = iterator(obj);
//  * let jsArray = [...objIterator];     //number[]
//  * ```
//  */
// export function iterator<T extends CSObject & IEnumeratorType<TValue>, TValue = any>(instance: T): T & IEnumerator$1Iterator<TValue>;
// //方法实现
// export function iterator(): object {
//     const instance = arguments[0] as CSObject;
//     if (typeof instance !== "object" || instance === null || instance === void 0) {
//         return instance;
//     }
//     if (!(instance instanceof CSObject)) {
//         throw new Error(`invalid parameter. Need a chsarp object`);
//     }
//     const prototype = Object.getPrototypeOf(instance);
//     if (!(ITERATOR_DEFINE in prototype)) {
//         const Type = instance.GetType();
//         if ($typeof(CSArray).IsAssignableFrom(Type)) {
//             defineArrayIterator(prototype);
//         }
//         else {
//             let type = Type, define = false;
//             while (type && !define) {
//                 let fullname = type.FullName;
//                 if (!fullname) break;
//                 if (fullname.startsWith("System.Collections.Generic.List`1[")) {
//                     defineListIterator(prototype);
//                     define = true;
//                 }
//                 else if (fullname.startsWith("System.Collections.Generic.Dictionary`2[")) {
//                     defineDictionaryIterator(prototype);
//                     define = true;
//                 }
//                 type = type.BaseType;
//             }
//             if (!define) {
//                 if ("GetEnumerator" in instance && typeof instance["GetEnumerator"] === "function") {
//                     defineIEnumeratorIterator(prototype);
//                 } else {
//                     throw new Error(`unsupported chsarp type: ${Type.FullName}`);
//                 }
//             }
//         }
//         prototype[ITERATOR_DEFINE] = true;
//     }
//     return instance;
// }
// export function test() {
//     const execute = (title: string, fn: () => void) => {
//         try {
//             console.log(`=====================${title}=====================`);
//             fn();
//             console.log(`===================== success ====================`);
//         } catch (e) {
//             console.log(`=====================  <color=red>fail</color>  =====================\n${e}`);
//         }
//     };
//     execute(`Array`, () => {
//         let obj = CS.System.Array.CreateInstance($typeof(CS.System.Int32), 5) as CS.System.Array$1<number>;
//         obj.set_Item(0, 1);
//         obj.set_Item(1, 2);
//         obj.set_Item(2, 3);
//         console.log(JSON.stringify([...iterator(obj)]));
//         iterator(obj).forEach((value, index) => {
//             console.log(index, value);
//         });
//     });
//     execute(`List`, () => {
//         const List_Number = $generic(CS.System.Collections.Generic.List$1, CS.System.Int32) as {
//             new(): CS.System.Collections.Generic.List$1<number>;
//         };
//         let obj = new List_Number();
//         obj.Add(4);
//         obj.Add(5);
//         obj.Add(6);
//         console.log(JSON.stringify([...iterator(obj)]));
//         iterator(obj).forEach((value, index) => {
//             console.log(index, value);
//         });
//     });
//     execute(`Dictionary`, () => {
//         const Dictionary_Number_String = $generic(CS.System.Collections.Generic.Dictionary$2, CS.System.Int32, CS.System.String) as {
//             new(): CS.System.Collections.Generic.Dictionary$2<number, string>;
//         };
//         let obj = new Dictionary_Number_String();
//         obj.set_Item(1, `message 1`);
//         obj.set_Item(2, `message 2`);
//         obj.set_Item(3, `message 3`);
//         console.log(JSON.stringify(iterator(obj).getKeys()));
//         console.log(JSON.stringify(iterator(obj).getValues()));
//         console.log(JSON.stringify([...iterator(obj)]));
//         iterator(obj).forEach((value, key) => {
//             console.log(key, value);
//         });
//         for (let [key, value] of iterator(obj)) {
//             console.log(key, value);
//         }
//     });
//     execute(`Hashtable`, () => {
//         let obj = new CS.System.Collections.Hashtable();
//         obj.Add(`key1`, `message 1`);
//         obj.Add(`key2`, `message 2`);
//         obj.Add(`key3`, `message 3`);
//         console.log(JSON.stringify([...iterator(obj)]));
//         iterator(obj).forEach((value, key) => {
//             console.log(key, value);
//         });
//     });
// }
