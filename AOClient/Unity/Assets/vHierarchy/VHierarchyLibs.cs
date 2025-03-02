#if UNITY_EDITOR
using Type = System.Type;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
using UnityEngine.Experimental.Rendering;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;


namespace VHierarchy.Libs
{
    public static class VUtils
    {
        #region Text


        public static string FormatDistance(float meters)
        {
            int m = (int)meters;

            if (m < 1000) return m + " m";
            else return (m / 1000) + "." + (m / 100) % 10 + " km";
        }
        public static string FormatLong(long l) => System.String.Format("{0:n0}", l);
        public static string FormatInt(int l) => FormatLong((long)l);
        public static string FormatSize(long bytes, bool sizeUnknownIfNotMoreThanZero = false)
        {
            if (sizeUnknownIfNotMoreThanZero && bytes == 0) return "Size unknown";

            var ss = new[] { "B", "KB", "MB", "GB", "TB" };
            var bprev = bytes;
            int i = 0;
            while (bytes >= 1024 && i++ < ss.Length - 1) bytes = (bprev = bytes) / 1024;

            if (bytes < 0) return "? B";
            if (i < 3) return string.Format("{0:0.#} ", bytes) + ss[i];
            return string.Format("{0:0.##} ", bytes) + ss[i];
        }
        public static string FormatTime(long ms, bool includeMs = false)
        {
            System.TimeSpan t = System.TimeSpan.FromMilliseconds(ms);
            var s = "";
            if (t.Hours != 0) s += " " + t.Hours + " hour" + CountSuffix(t.Hours);
            if (t.Minutes != 0) s += " " + t.Minutes + " minute" + CountSuffix(t.Minutes);
            if (t.Seconds != 0) s += " " + t.Seconds + " second" + CountSuffix(t.Seconds);
            if (t.Milliseconds != 0 && includeMs) s += " " + t.Milliseconds + " millisecond" + CountSuffix(t.Milliseconds);

            if (s == "")
                if (includeMs) s = "0 milliseconds";
                else s = "0 seconds";

            return s.Trim();
        }
        static string CountSuffix(long c) => c % 10 != 1 ? "s" : "";
        public static string Remove(this string s, string toRemove)
        {
            if (toRemove == "") return s;
            return s.Replace(toRemove, "");
        }

        public static bool IsEmpty(this string s) => s == "";
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);





        #endregion

        #region IEnumerables


        public static T AddAt<T>(this List<T> l, T r, int i)
        {
            if (i < 0) i = 0;
            if (i >= l.Count)
                l.Add(r);
            else
                l.Insert(i, r);
            return r;
        }
        public static T RemoveLast<T>(this List<T> l)
        {
            if (!l.Any()) return default;

            var r = l.Last();

            l.RemoveAt(l.Count - 1);

            return r;
        }

        public static void Add<T>(this List<T> list, params T[] items)
        {
            foreach (var r in items)
                list.Add(r);
        }

        public static int LastIndex<T>(this List<T> l) => l.Count - 1; // toremove

        // public static T GetAtWrapped<T>(this List<T> list, int i) // toremove
        // {
        //     while (i < 0) i += list.Count;
        //     while (i >= list.Count) i -= list.Count;

        //     return list[i];
        // }





        #endregion

        #region Linq


        public static T NextTo<T>(this IEnumerable<T> e, T to) => e.SkipWhile(r => !r.Equals(to)).Skip(1).FirstOrDefault();

        public static T PreviousTo<T>(this IEnumerable<T> e, T to) => e.Reverse().SkipWhile(r => !r.Equals(to)).Skip(1).FirstOrDefault();

        public static T NextToOtFirst<T>(this IEnumerable<T> e, T to) => e.NextTo(to) ?? e.First();

        public static T PreviousToOrLast<T>(this IEnumerable<T> e, T to) => e.PreviousTo(to) ?? e.Last();



        public static Dictionary<TKey, TValue> MergeDictionaries<TKey, TValue>(IEnumerable<Dictionary<TKey, TValue>> dicts)
        {
            if (dicts.Count() == 0) return null;
            if (dicts.Count() == 1) return dicts.First();

            var mergedDict = new Dictionary<TKey, TValue>(dicts.First());

            foreach (var dict in dicts.Skip(1))
                foreach (var r in dict)
                    if (!mergedDict.ContainsKey(r.Key))
                        mergedDict.Add(r.Key, r.Value);

            return mergedDict;
        }

        public static IEnumerable<T> InsertFirst<T>(this IEnumerable<T> ie, T t) => new[] { t }.Concat(ie);


        public static bool None<T>(this IEnumerable<T> ie, System.Func<T, bool> f) => !ie.Any(f);

        public static bool None<T>(this IEnumerable<T> ie) => !ie.Any();


        public static int IndexOfFirst<T>(this List<T> list, System.Func<T, bool> f) => list.FirstOrDefault(f) is T t ? list.IndexOf(t) : -1;

        public static int IndexOfLast<T>(this List<T> list, System.Func<T, bool> f) => list.LastOrDefault(f) is T t ? list.IndexOf(t) : -1;


        public static void SortBy<T, T2>(this List<T> list, System.Func<T, T2> keySelector) where T2 : System.IComparable => list.Sort((q, w) => keySelector(q).CompareTo(keySelector(w)));

        public static void RemoveValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary.FirstOrDefault(r => r.Value.Equals(value)) is var kvp)
                dictionary.Remove(kvp);
        }




        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key) => dic.ContainsKey(key) ? dic[key] : default;





        #endregion

        #region Reflection

        public static object GetFieldValue(this object o, string fieldName, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(fieldName) is FieldInfo fieldInfo)
                return fieldInfo.GetValue(target);


            if (exceptionIfNotFound)
                throw new System.Exception($"Field '{fieldName}' not found in '{type.Name}' type and its parent types");

            return null;

        }
        public static object GetPropertyValue(this object o, string propertyName, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetPropertyInfo(propertyName) is PropertyInfo propertyInfo)
                return propertyInfo.GetValue(target);


            if (exceptionIfNotFound)
                throw new System.Exception($"Property '{propertyName}' not found in '{type.Name}' type and its parent types");

            return null;

        }
        public static object GetMemberValue(this object o, string memberName, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(memberName) is FieldInfo fieldInfo)
                return fieldInfo.GetValue(target);

            if (type.GetPropertyInfo(memberName) is PropertyInfo propertyInfo)
                return propertyInfo.GetValue(target);


            if (exceptionIfNotFound)
                throw new System.Exception($"Member '{memberName}' not found in '{type.Name}' type and its parent types");

            return null;

        }

        public static void SetFieldValue(this object o, string fieldName, object value, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(fieldName) is FieldInfo fieldInfo)
                fieldInfo.SetValue(target, value);


            else if (exceptionIfNotFound)
                throw new System.Exception($"Field '{fieldName}' not found in '{type.Name}' type and its parent types");

        }
        public static void SetPropertyValue(this object o, string propertyName, object value, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetPropertyInfo(propertyName) is PropertyInfo propertyInfo)
                propertyInfo.SetValue(target, value);


            else if (exceptionIfNotFound)
                throw new System.Exception($"Property '{propertyName}' not found in '{type.Name}' type and its parent types");

        }
        public static void SetMemberValue(this object o, string memberName, object value, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(memberName) is FieldInfo fieldInfo)
                fieldInfo.SetValue(target, value);

            else if (type.GetPropertyInfo(memberName) is PropertyInfo propertyInfo)
                propertyInfo.SetValue(target, value);


            else if (exceptionIfNotFound)
                throw new System.Exception($"Member '{memberName}' not found in '{type.Name}' type and its parent types");

        }

        public static object InvokeMethod(this object o, string methodName, params object[] parameters)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetMethodInfo(methodName, parameters.Select(r => r.GetType()).ToArray()) is MethodInfo methodInfo)
                return methodInfo.Invoke(target, parameters);


            throw new System.Exception($"Method '{methodName}' not found in '{type.Name}' type, its parent types and interfaces");

        }



        static FieldInfo GetFieldInfo(this Type type, string fieldName)
        {
            if (fieldInfoCache.TryGetValue(type, out var fieldInfosByNames))
                if (fieldInfosByNames.TryGetValue(fieldName, out var fieldInfo))
                    return fieldInfo;


            if (!fieldInfoCache.ContainsKey(type))
                fieldInfoCache[type] = new Dictionary<string, FieldInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetField(fieldName, maxBindingFlags) is FieldInfo fieldInfo)
                    return fieldInfoCache[type][fieldName] = fieldInfo;


            return fieldInfoCache[type][fieldName] = null;

        }
        static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();

        static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            if (propertyInfoCache.TryGetValue(type, out var propertyInfosByNames))
                if (propertyInfosByNames.TryGetValue(propertyName, out var propertyInfo))
                    return propertyInfo;


            if (!propertyInfoCache.ContainsKey(type))
                propertyInfoCache[type] = new Dictionary<string, PropertyInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetProperty(propertyName, maxBindingFlags) is PropertyInfo propertyInfo)
                    return propertyInfoCache[type][propertyName] = propertyInfo;


            return propertyInfoCache[type][propertyName] = null;

        }
        static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        static MethodInfo GetMethodInfo(this Type type, string methodName, params Type[] argumentTypes)
        {
            var methodHash = methodName.GetHashCode() ^ argumentTypes.Aggregate(0, (hash, r) => hash ^= r.GetHashCode());


            if (methodInfoCache.TryGetValue(type, out var methodInfosByHashes))
                if (methodInfosByHashes.TryGetValue(methodHash, out var methodInfo))
                    return methodInfo;



            if (!methodInfoCache.ContainsKey(type))
                methodInfoCache[type] = new Dictionary<int, MethodInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetMethod(methodName, maxBindingFlags, null, argumentTypes, null) is MethodInfo methodInfo)
                    return methodInfoCache[type][methodHash] = methodInfo;

            foreach (var interfaceType in type.GetInterfaces())
                if (interfaceType.GetMethod(methodName, maxBindingFlags, null, argumentTypes, null) is MethodInfo methodInfo)
                    return methodInfoCache[type][methodHash] = methodInfo;



            return methodInfoCache[type][methodHash] = null;

        }
        static Dictionary<Type, Dictionary<int, MethodInfo>> methodInfoCache = new Dictionary<Type, Dictionary<int, MethodInfo>>();



        public static T GetFieldValue<T>(this object o, string fieldName, bool exceptionIfNotFound = true) => (T)o.GetFieldValue(fieldName, exceptionIfNotFound);
        public static T GetPropertyValue<T>(this object o, string propertyName, bool exceptionIfNotFound = true) => (T)o.GetPropertyValue(propertyName, exceptionIfNotFound);
        public static T GetMemberValue<T>(this object o, string memberName, bool exceptionIfNotFound = true) => (T)o.GetMemberValue(memberName, exceptionIfNotFound);
        public static T InvokeMethod<T>(this object o, string methodName, params object[] parameters) => (T)o.InvokeMethod(methodName, parameters);






        public static List<Type> GetSubclasses(this Type t) => t.Assembly.GetTypes().Where(type => type.IsSubclassOf(t)).ToList();

        public static object GetDefaultValue(this FieldInfo f, params object[] constructorVars) => f.GetValue(System.Activator.CreateInstance(((MemberInfo)f).ReflectedType, constructorVars));

        public static object GetDefaultValue(this FieldInfo f) => f.GetValue(System.Activator.CreateInstance(((MemberInfo)f).ReflectedType));


        public static IEnumerable<FieldInfo> GetFieldsWithoutBase(this Type t) => t.GetFields().Where(r => !t.BaseType.GetFields().Any(rr => rr.Name == r.Name));

        public static IEnumerable<PropertyInfo> GetPropertiesWithoutBase(this Type t) => t.GetProperties().Where(r => !t.BaseType.GetProperties().Any(rr => rr.Name == r.Name));


        public const BindingFlags maxBindingFlags = (BindingFlags)62;







        #endregion

        #region Math


        public static bool Approx(this float f1, float f2) => Mathf.Approximately(f1, f2);
        public static bool CloseTo(this float f1, float f2, float distance) => f1.DistTo(f2) <= distance;
        public static float DistTo(this float f1, float f2) => Mathf.Abs(f1 - f2);
        public static float Dist(float f1, float f2) => Mathf.Abs(f1 - f2);
        public static float Avg(float f1, float f2) => (f1 + f2) / 2;
        public static float Abs(this float f) => Mathf.Abs(f);
        public static int Abs(this int f) => Mathf.Abs(f);
        public static float Sign(this float f) => Mathf.Sign(f);
        public static float Clamp(this float f, float f0, float f1) => Mathf.Clamp(f, f0, f1);
        public static int Clamp(this int f, int f0, int f1) => Mathf.Clamp(f, f0, f1);
        public static float Clamp01(this float f) => Mathf.Clamp(f, 0, 1);
        public static Vector2 Clamp01(this Vector2 f) => new Vector2(f.x.Clamp01(), f.y.Clamp01());
        public static Vector3 Clamp01(this Vector3 f) => new Vector3(f.x.Clamp01(), f.y.Clamp01(), f.z.Clamp01());


        public static float Pow(this float f, float pow) => Mathf.Pow(f, pow);
        public static int Pow(this int f, int pow) => (int)Mathf.Pow(f, pow);

        public static float Round(this float f) => Mathf.Round(f);
        public static float Ceil(this float f) => Mathf.Ceil(f);
        public static float Floor(this float f) => Mathf.Floor(f);
        public static int RoundToInt(this float f) => Mathf.RoundToInt(f);
        public static int CeilToInt(this float f) => Mathf.CeilToInt(f);
        public static int FloorToInt(this float f) => Mathf.FloorToInt(f);
        public static int ToInt(this float f) => (int)f;
        public static float ToFloat(this int f) => (float)f;


        public static float Sqrt(this float f) => Mathf.Sqrt(f);

        public static float Max(this float f, float ff) => Mathf.Max(f, ff);
        public static float Min(this float f, float ff) => Mathf.Min(f, ff);
        public static int Max(this int f, int ff) => Mathf.Max(f, ff);
        public static int Min(this int f, int ff) => Mathf.Min(f, ff);

        public static float Loop(this float f, float boundMin, float boundMax)
        {
            while (f < boundMin) f += boundMax - boundMin;
            while (f > boundMax) f -= boundMax - boundMin;
            return f;
        }
        public static float Loop(this float f, float boundMax) => f.Loop(0, boundMax);

        public static float PingPong(this float f, float boundMin, float boundMax) => boundMin + Mathf.PingPong(f - boundMin, boundMax - boundMin);
        public static float PingPong(this float f, float boundMax) => f.PingPong(0, boundMax);


        public static float TriangleArea(Vector2 A, Vector2 B, Vector2 C) => Vector3.Cross(A - B, A - C).z.Abs() / 2;
        public static Vector2 LineIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            var a1 = B.y - A.y;
            var b1 = A.x - B.x;
            var c1 = a1 * A.x + b1 * A.y;

            var a2 = D.y - C.y;
            var b2 = C.x - D.x;
            var c2 = a2 * C.x + b2 * C.y;

            var d = a1 * b2 - a2 * b1;

            var x = (b2 * c1 - b1 * c2) / d;
            var y = (a1 * c2 - a2 * c1) / d;

            return new Vector2(x, y);

        }

        public static float ProjectOn(this Vector2 v, Vector2 on) => Vector3.Project(v, on).magnitude;
        public static float AngleTo(this Vector2 v, Vector2 to) => Vector2.Angle(v, to);

        public static Vector2 Rotate(this Vector2 v, float deg) => Quaternion.AngleAxis(deg, Vector3.forward) * v;

        public static float Smoothstep(this float f) { f = f.Clamp01(); return f * f * (3 - 2 * f); }

        public static float InverseLerp(this Vector2 v, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            var av = v - a;
            return Vector2.Dot(av, ab) / Vector2.Dot(ab, ab);
        }

        public static bool IsOdd(this int i) => i % 2 == 1;
        public static bool IsEven(this int i) => i % 2 == 0;

        public static bool IsInRange(this int i, int a, int b) => i >= a && i <= b;
        public static bool IsInRange(this float i, float a, float b) => i >= a && i <= b;

        public static bool IsInRangeOf(this int i, IList list) => i.IsInRange(0, list.Count - 1);
        public static bool IsInRangeOf<T>(this int i, T[] array) => i.IsInRange(0, array.Length - 1);







        #endregion

        #region Lerping


        public static float LerpT(float lerpSpeed, float deltaTime) => 1 - Mathf.Exp(-lerpSpeed * 2f * deltaTime);
        public static float LerpT(float lerpSpeed) => LerpT(lerpSpeed, Time.deltaTime);

        public static float Lerp(float f1, float f2, float t) => Mathf.LerpUnclamped(f1, f2, t);
        public static float Lerp(ref float f1, float f2, float t) => f1 = Lerp(f1, f2, t);

        public static Vector2 Lerp(Vector2 f1, Vector2 f2, float t) => Vector2.LerpUnclamped(f1, f2, t);
        public static Vector2 Lerp(ref Vector2 f1, Vector2 f2, float t) => f1 = Lerp(f1, f2, t);

        public static Vector3 Lerp(Vector3 f1, Vector3 f2, float t) => Vector3.LerpUnclamped(f1, f2, t);
        public static Vector3 Lerp(ref Vector3 f1, Vector3 f2, float t) => f1 = Lerp(f1, f2, t);

        public static Color Lerp(Color f1, Color f2, float t) => Color.LerpUnclamped(f1, f2, t);
        public static Color Lerp(ref Color f1, Color f2, float t) => f1 = Lerp(f1, f2, t);


        public static float Lerp(float current, float target, float speed, float deltaTime) => Mathf.Lerp(current, target, LerpT(speed, deltaTime));
        public static float Lerp(ref float current, float target, float speed, float deltaTime) => current = Lerp(current, target, speed, deltaTime);

        public static Vector2 Lerp(Vector2 current, Vector2 target, float speed, float deltaTime) => Vector2.Lerp(current, target, LerpT(speed, deltaTime));
        public static Vector2 Lerp(ref Vector2 current, Vector2 target, float speed, float deltaTime) => current = Lerp(current, target, speed, deltaTime);

        public static Vector3 Lerp(Vector3 current, Vector3 target, float speed, float deltaTime) => Vector3.Lerp(current, target, LerpT(speed, deltaTime));
        public static Vector3 Lerp(ref Vector3 current, Vector3 target, float speed, float deltaTime) => current = Lerp(current, target, speed, deltaTime);

        public static float SmoothDamp(float current, float target, float speed, ref float derivative, float deltaTime) => Mathf.SmoothDamp(current, target, ref derivative, .5f / speed, Mathf.Infinity, deltaTime);
        public static float SmoothDamp(ref float current, float target, float speed, ref float derivative, float deltaTime) => current = SmoothDamp(current, target, speed, ref derivative, deltaTime);
        public static float SmoothDamp(float current, float target, float speed, ref float derivative) => SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
        public static float SmoothDamp(ref float current, float target, float speed, ref float derivative) => current = SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);

        public static Vector2 SmoothDamp(Vector2 current, Vector2 target, float speed, ref Vector2 derivative, float deltaTime) => Vector2.SmoothDamp(current, target, ref derivative, .5f / speed, Mathf.Infinity, deltaTime);
        public static Vector2 SmoothDamp(ref Vector2 current, Vector2 target, float speed, ref Vector2 derivative, float deltaTime) => current = SmoothDamp(current, target, speed, ref derivative, deltaTime);
        public static Vector2 SmoothDamp(Vector2 current, Vector2 target, float speed, ref Vector2 derivative) => SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
        public static Vector2 SmoothDamp(ref Vector2 current, Vector2 target, float speed, ref Vector2 derivative) => current = SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);

        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, float speed, ref Vector3 derivative, float deltaTime) => Vector3.SmoothDamp(current, target, ref derivative, .5f / speed, Mathf.Infinity, deltaTime);
        public static Vector3 SmoothDamp(ref Vector3 current, Vector3 target, float speed, ref Vector3 derivative, float deltaTime) => current = SmoothDamp(current, target, speed, ref derivative, deltaTime);
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, float speed, ref Vector3 derivative) => SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);
        public static Vector3 SmoothDamp(ref Vector3 current, Vector3 target, float speed, ref Vector3 derivative) => current = SmoothDamp(current, target, speed, ref derivative, Time.deltaTime);






        #endregion

        #region Colors


        public static Color HSLToRGB(float h, float s, float l)
        {
            float hue2Rgb(float v1, float v2, float vH)
            {
                if (vH < 0f)
                    vH += 1f;

                if (vH > 1f)
                    vH -= 1f;

                if (6f * vH < 1f)
                    return v1 + (v2 - v1) * 6f * vH;

                if (2f * vH < 1f)
                    return v2;

                if (3f * vH < 2f)
                    return v1 + (v2 - v1) * (2f / 3f - vH) * 6f;

                return v1;
            }

            if (s.Approx(0)) return new Color(l, l, l);

            float k1;

            if (l < .5f)
                k1 = l * (1f + s);
            else
                k1 = l + s - s * l;


            var k2 = 2f * l - k1;

            float r, g, b;
            r = hue2Rgb(k2, k1, h + 1f / 3);
            g = hue2Rgb(k2, k1, h);
            b = hue2Rgb(k2, k1, h - 1f / 3);

            return new Color(r, g, b);
        }
        public static Color LCHtoRGB(float l, float c, float h)
        {
            l *= 100;
            c *= 100;
            h *= 360;

            double xw = 0.948110;
            double yw = 1.00000;
            double zw = 1.07304;

            float a = c * Mathf.Cos(Mathf.Deg2Rad * h);
            float b = c * Mathf.Sin(Mathf.Deg2Rad * h);

            float fy = (l + 16) / 116;
            float fx = fy + (a / 500);
            float fz = fy - (b / 200);

            float x = (float)System.Math.Round(xw * ((System.Math.Pow(fx, 3) > 0.008856) ? System.Math.Pow(fx, 3) : ((fx - 16 / 116) / 7.787)), 5);
            float y = (float)System.Math.Round(yw * ((System.Math.Pow(fy, 3) > 0.008856) ? System.Math.Pow(fy, 3) : ((fy - 16 / 116) / 7.787)), 5);
            float z = (float)System.Math.Round(zw * ((System.Math.Pow(fz, 3) > 0.008856) ? System.Math.Pow(fz, 3) : ((fz - 16 / 116) / 7.787)), 5);

            float r = x * 3.2406f - y * 1.5372f - z * 0.4986f;
            float g = -x * 0.9689f + y * 1.8758f + z * 0.0415f;
            float bValue = x * 0.0557f - y * 0.2040f + z * 1.0570f;

            r = r > 0.0031308f ? 1.055f * (float)System.Math.Pow(r, 1 / 2.4) - 0.055f : r * 12.92f;
            g = g > 0.0031308f ? 1.055f * (float)System.Math.Pow(g, 1 / 2.4) - 0.055f : g * 12.92f;
            bValue = bValue > 0.0031308f ? 1.055f * (float)System.Math.Pow(bValue, 1 / 2.4) - 0.055f : bValue * 12.92f;

            // r = (float)System.Math.Round(System.Math.Max(0, System.Math.Min(1, r)));
            // g = (float)System.Math.Round(System.Math.Max(0, System.Math.Min(1, g)));
            // bValue = (float)System.Math.Round(System.Math.Max(0, System.Math.Min(1, bValue)));

            return new Color(r, g, bValue);

        }



        public static Color Greyscale(float brightness, float alpha = 1) => new Color(brightness, brightness, brightness, alpha);

        public static Color SetAlpha(this Color color, float alpha) { color.a = alpha; return color; }
        public static Color MultiplyAlpha(this Color color, float k) { color.a *= k; return color; }





        #endregion

        #region Rects


        public static Rect Resize(this Rect rect, float px) { rect.x += px; rect.y += px; rect.width -= px * 2; rect.height -= px * 2; return rect; }

        public static Rect SetPos(this Rect rect, Vector2 v) => rect.SetPos(v.x, v.y);
        public static Rect SetPos(this Rect rect, float x, float y) { rect.x = x; rect.y = y; return rect; }

        public static Rect SetX(this Rect rect, float x) => rect.SetPos(x, rect.y);
        public static Rect SetY(this Rect rect, float y) => rect.SetPos(rect.x, y);
        public static Rect SetXMax(this Rect rect, float xMax) { rect.xMax = xMax; return rect; }
        public static Rect SetYMax(this Rect rect, float yMax) { rect.yMax = yMax; return rect; }

        public static Rect SetMidPos(this Rect r, Vector2 v) => r.SetPos(v).MoveX(-r.width / 2).MoveY(-r.height / 2);

        public static Rect Move(this Rect rect, Vector2 v) { rect.position += v; return rect; }
        public static Rect Move(this Rect rect, float x, float y) { rect.x += x; rect.y += y; return rect; }
        public static Rect MoveX(this Rect rect, float px) { rect.x += px; return rect; }
        public static Rect MoveY(this Rect rect, float px) { rect.y += px; return rect; }

        public static Rect SetWidth(this Rect rect, float f) { rect.width = f; return rect; }
        public static Rect SetWidthFromMid(this Rect rect, float px) { rect.x += rect.width / 2; rect.width = px; rect.x -= rect.width / 2; return rect; }
        public static Rect SetWidthFromRight(this Rect rect, float px) { rect.x += rect.width; rect.width = px; rect.x -= rect.width; return rect; }

        public static Rect SetHeight(this Rect rect, float f) { rect.height = f; return rect; }
        public static Rect SetHeightFromMid(this Rect rect, float px) { rect.y += rect.height / 2; rect.height = px; rect.y -= rect.height / 2; return rect; }
        public static Rect SetHeightFromBottom(this Rect rect, float px) { rect.y += rect.height; rect.height = px; rect.y -= rect.height; return rect; }

        public static Rect AddWidth(this Rect rect, float f) => rect.SetWidth(rect.width + f);
        public static Rect AddWidthFromMid(this Rect rect, float f) => rect.SetWidthFromMid(rect.width + f);
        public static Rect AddWidthFromRight(this Rect rect, float f) => rect.SetWidthFromRight(rect.width + f);

        public static Rect AddHeight(this Rect rect, float f) => rect.SetHeight(rect.height + f);
        public static Rect AddHeightFromMid(this Rect rect, float f) => rect.SetHeightFromMid(rect.height + f);
        public static Rect AddHeightFromBottom(this Rect rect, float f) => rect.SetHeightFromBottom(rect.height + f);

        public static Rect SetSize(this Rect rect, Vector2 v) => rect.SetWidth(v.x).SetHeight(v.y);
        public static Rect SetSize(this Rect rect, float w, float h) => rect.SetWidth(w).SetHeight(h);
        public static Rect SetSize(this Rect rect, float f) { rect.height = rect.width = f; return rect; }

        public static Rect SetSizeFromMid(this Rect r, Vector2 v) => r.Move(r.size / 2).SetSize(v).Move(-v / 2);
        public static Rect SetSizeFromMid(this Rect r, float x, float y) => r.SetSizeFromMid(new Vector2(x, y));
        public static Rect SetSizeFromMid(this Rect r, float f) => r.SetSizeFromMid(new Vector2(f, f));

        public static Rect AlignToPixelGrid(this Rect r) => GUIUtility.AlignRectToDevice(r);





        #endregion

        #region Vectors


        public static Vector2 AddX(this Vector2 v, float f) => new Vector2(v.x + f, v.y + 0);
        public static Vector2 AddY(this Vector2 v, float f) => new Vector2(v.x + 0, v.y + f);

        public static Vector3 AddX(this Vector3 v, float f) => new Vector3(v.x + f, v.y + 0, v.z + 0);
        public static Vector3 AddY(this Vector3 v, float f) => new Vector3(v.x + 0, v.y + f, v.z + 0);
        public static Vector3 AddZ(this Vector3 v, float f) => new Vector3(v.x + 0, v.y + 0, v.z + f);

        public static Vector2 xx(this Vector3 v) { return new Vector2(v.x, v.x); }
        public static Vector2 xy(this Vector3 v) { return new Vector2(v.x, v.y); }
        public static Vector2 xz(this Vector3 v) { return new Vector2(v.x, v.z); }
        public static Vector2 yx(this Vector3 v) { return new Vector2(v.y, v.x); }
        public static Vector2 yy(this Vector3 v) { return new Vector2(v.y, v.y); }
        public static Vector2 yz(this Vector3 v) { return new Vector2(v.y, v.z); }
        public static Vector2 zx(this Vector3 v) { return new Vector2(v.z, v.x); }
        public static Vector2 zy(this Vector3 v) { return new Vector2(v.z, v.y); }
        public static Vector2 zz(this Vector3 v) { return new Vector2(v.z, v.z); }





        #endregion

        #region Textures


        public static Texture2D CreateTexture2D(int width, int height, GraphicsFormat graphicsFormat = GraphicsFormat.R8G8B8A8_SRGB, bool useMips = false)
        {
            return new Texture2D(width, height, graphicsFormat, useMips ? TextureCreationFlags.MipChain : TextureCreationFlags.None);
        }

        public static RenderTexture CreateRT(int width, int height, GraphicsFormat graphicsFormat = GraphicsFormat.R8G8B8A8_SRGB, bool useMips = false, bool autoGenerateMips = true, bool useDepth = false)
        {
            var rt = new RenderTexture(width, height, useDepth ? 24 : 0, graphicsFormat);

            rt.useMipMap = useMips;
            rt.autoGenerateMips = autoGenerateMips;

            rt.enableRandomWrite = true;

            return rt;

        }
        public static RenderTexture GetTemporaryRT(int width, int height, GraphicsFormat graphicsFormat = GraphicsFormat.R8G8B8A8_SRGB, bool useMips = false, bool autoGenerateMips = true, bool useDepth = false)
        {
            var rt = RenderTexture.GetTemporary(width, height, useDepth ? 24 : 0, graphicsFormat);

            rt.useMipMap = useMips;
            rt.autoGenerateMips = autoGenerateMips;

            rt.enableRandomWrite = true;

            return rt;

        }

        public static RenderTexture CreateRT(this RenderTextureDescriptor descriptor) => new RenderTexture(descriptor);
        public static RenderTexture CreateRT(this RenderTextureDescriptor descriptor, int resolution)
        {
            descriptor.width = descriptor.height = resolution;

            return descriptor.CreateRT();

        }
        public static RenderTexture CreateRT(this RenderTextureDescriptor descriptor, int width, int height)
        {
            descriptor.width = width;
            descriptor.height = height;

            return descriptor.CreateRT();

        }
        public static RenderTexture CreateRT(this RenderTextureDescriptor descriptor, float resolution) => descriptor.GetTemporaryRT(Mathf.RoundToInt(resolution));
        public static RenderTexture CreateRT(this RenderTextureDescriptor descriptor, float width, float height) => descriptor.CreateRT(Mathf.RoundToInt(width), Mathf.RoundToInt(height));
        public static RenderTexture GetTemporaryRT(this RenderTextureDescriptor descriptor) => RenderTexture.GetTemporary(descriptor);
        public static RenderTexture GetTemporaryRT(this RenderTextureDescriptor descriptor, int resolution)
        {
            descriptor.width = descriptor.height = resolution;

            return descriptor.GetTemporaryRT();

        }
        public static RenderTexture GetTemporaryRT(this RenderTextureDescriptor descriptor, int width, int height)
        {
            descriptor.width = width;
            descriptor.height = height;

            return descriptor.GetTemporaryRT();

        }
        public static RenderTexture GetTemporaryRT(this RenderTextureDescriptor descriptor, float resolution) => descriptor.GetTemporaryRT(Mathf.RoundToInt(resolution));
        public static RenderTexture GetTemporaryRT(this RenderTextureDescriptor descriptor, float width, float height) => descriptor.GetTemporaryRT(Mathf.RoundToInt(width), Mathf.RoundToInt(height));

        public static void ReleaseTemporary(this RenderTexture rt) { if (rt) RenderTexture.ReleaseTemporary(rt); }



        public static Texture2D ToTexture2D(this RenderTexture rt)
        {
            var texture2D = CreateTexture2D(rt.width, rt.height, rt.graphicsFormat, rt.useMipMap);

            texture2D.ReadPixelsFrom(rt);
            texture2D.Apply();

            return texture2D;

        }
        public static RenderTexture ToRenderTexture(this Texture2D texture2d)
        {
            var rt = CreateRT(texture2d.width, texture2d.height, texture2d.graphicsFormat, texture2d.mipmapCount > 1);

            Graphics.CopyTexture(texture2d, rt);

            return rt;

        }


        public static void ReadPixelsFrom(this Texture2D texture2D, RenderTexture renderTexture)
        {
            var prevActive = RenderTexture.active;

            RenderTexture.active = renderTexture;

            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            RenderTexture.active = prevActive;

        }
        // public static void CopyTo(this RenderTexture source, Texture2D target) // todo to readpixels overload
        // {
        //     var prevActive = RenderTexture.active;

        //     RenderTexture.active = source;

        //     target.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
        //     target.Apply();

        //     RenderTexture.active = prevActive;


        //     // somewhere in unity source code reading is done like this, but it throws out of bounds read exception on win:

        //     // if (!SystemInfo.graphicsUVStartsAtTop || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
        //     //     texture2d.ReadPixels(new Rect(0, 0, texture2d.width, texture2d.height), 0, 0);
        //     // else
        //     //     texture2d.ReadPixels(new Rect(0, texture2d.height, texture2d.width, texture2d.height), 0, 0);

        //     // this was used in the legacy rt.CopyToTexture2D extension method

        // }

        public static Texture2D CreateCopy(this Texture2D texture2d)
        {
            var copy = CreateTexture2D(texture2d.width, texture2d.height, texture2d.graphicsFormat, texture2d.mipmapCount > 1);

            Graphics.CopyTexture(texture2d, copy);

            return copy;

        }
        public static Texture2D CreateResizedCopy(this Texture2D texture2d, int w, int h)
        {
            var rt = GetTemporaryRT(w, h, texture2d.graphicsFormat.GetCompatibleForRendering(), false);

            Graphics.Blit(texture2d, rt);


            var resizedCopy = CreateTexture2D(w, h, texture2d.graphicsFormat.GetCompatibleForRendering(), texture2d.mipmapCount > 1);

            resizedCopy.ReadPixelsFrom(rt);
            resizedCopy.Apply();

            if (RenderTexture.active == rt)
                RenderTexture.active = null;

            rt.ReleaseTemporary();


            return resizedCopy;

        }


        public static void FillWithColor(this Texture2D texture2d, Color color)
        {
            var pixels = new Color32[texture2d.width * texture2d.height];

            var color32 = (Color32)color;

            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color32;

            texture2d.SetPixels32(pixels);
            texture2d.Apply();

        }
        public static RenderTexture FillWithColor(this RenderTexture rt, Color color) // todo builtin shader or GL.clear
        {
            var mat = new Material(Shader.Find("Hidden/VBlitColor"));

            mat.SetColor("_color", color);

            Graphics.Blit(null, rt, mat);

            mat.Destroy();

            return rt;

        }



        public static GraphicsFormat GetCompatibleForRendering(this GraphicsFormat graphicsFormat)
        {
#if UNITY_2023_2_OR_NEWER
            return SystemInfo.GetCompatibleFormat(graphicsFormat, GraphicsFormatUsage.Render);
#else
            return SystemInfo.GetCompatibleFormat(graphicsFormat, FormatUsage.Render);
#endif

        }



#if UNITY_EDITOR

        public static void SavePNG(this Texture2D texture2d, string path) => File.WriteAllBytes(path, texture2d.EncodeToPNG());

        public static void SetImportSettings(this Texture2D texture2d, int? maxSize = null, bool? useMips = null, bool? sRGB = null, bool? isReadable = null, bool? useCompression = null)
        {
            var importer = texture2d.GetImporter();

            if (useCompression != null)
                importer.textureCompression = useCompression.GetValueOrDefault() ? TextureImporterCompression.Compressed : TextureImporterCompression.Uncompressed;

            if (sRGB != null)
                importer.sRGBTexture = sRGB.GetValueOrDefault();

            if (maxSize != null)
                importer.maxTextureSize = maxSize.GetValueOrDefault();

            if (useMips != null)
                importer.mipmapEnabled = useMips.GetValueOrDefault();


            // if (texture2d.format == TextureFormat.R16 || texture2d.format == TextureFormat.RG32)
            if (texture2d.format == TextureFormat.R16)
            {
                var platformSettings = importer.GetDefaultPlatformTextureSettings();

                platformSettings.format = TextureImporterFormat.R16;

                if (maxSize != null)
                    platformSettings.maxTextureSize = maxSize.GetValueOrDefault();

                importer.SetPlatformTextureSettings(platformSettings);

            }

        }

        public static TextureImporter GetImporter(this Texture2D t) => (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(t));

#endif







        #endregion

        #region Compute


        [System.Serializable]
        public class GaussianKernel
        {
            public GaussianKernel(bool isEvenSize = false, int radius = 7, float sharpness = .5f)
            {
                this.isEvenSize = isEvenSize;
                this.radius = radius;
                this.sharpness = sharpness;
            }

            public bool isEvenSize = false;

            public int radius = 7;
            public float sharpness = .5f;

            public int size => radius * 2 + (isEvenSize ? 0 : 1);
            public float sigma => 1 - Mathf.Pow(sharpness, .1f) * .99999f;

            public float[,] Array2d()
            {
                float[,] kr = new float[size, size];

                if (size == 1) { kr[0, 0] = 1; return kr; }

                var a = -2f * radius * radius / Mathf.Log(sigma);
                var sum = 0f;

                for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                    {
                        var rX = size % 2 == 1 ? (x - radius) : (x - radius) + .5f;
                        var rY = size % 2 == 1 ? (y - radius) : (y - radius) + .5f;
                        var dist = Mathf.Sqrt(rX * rX + rY * rY);
                        kr[x, y] = Mathf.Exp(-dist * dist / a);
                        sum += kr[x, y];
                    }

                for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                        kr[x, y] /= sum;

                return kr;
            }
            public float[] ArrayFlat()
            {
                var gk = Array2d();
                float[] flat = new float[size * size];

                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        flat[(i * size + j)] = gk[i, j];

                return flat;
            }
        }





        #endregion

        #region GameObjects


        public static bool IsPrefab(this GameObject go) => go.scene.name == null || go.scene.name == go.name;

        public static Bounds GetBounds(this GameObject go, bool local = false)
        {
            Bounds bounds = default;

            foreach (var r in go.GetComponentsInChildren<MeshRenderer>())
            {
                var b = local ? r.gameObject.GetComponent<MeshFilter>().sharedMesh.bounds : r.bounds;

                if (bounds == default)
                    bounds = b;
                else
                    bounds.Encapsulate(b);
            }

            foreach (var r in go.GetComponentsInChildren<Terrain>())
            {
                var b = local ? new Bounds(r.terrainData.size / 2, r.terrainData.size) : new Bounds(r.transform.position + r.terrainData.size / 2, r.terrainData.size);

                if (bounds == default)
                    bounds = b;
                else
                    bounds.Encapsulate(new Bounds(r.transform.position + r.terrainData.size / 2, r.terrainData.size));

            }

            if (bounds == default)
                bounds.center = go.transform.position;

            return bounds;
        }





        #endregion

        #region Objects


        public static Object[] FindObjects(Type type)
        {
#if UNITY_2023_1_OR_NEWER
            return Object.FindObjectsByType(type, FindObjectsSortMode.None);
#else
            return Object.FindObjectsOfType(type);
#endif
        }
        public static T[] FindObjects<T>() where T : Object
        {
#if UNITY_2023_1_OR_NEWER
            return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
#else
            return Object.FindObjectsOfType<T>();
#endif
        }

        public static void Destroy(this Object r)
        {
            if (Application.isPlaying)
                Object.Destroy(r);
            else
                Object.DestroyImmediate(r);

        }

        public static void DestroyImmediate(this Object o) => Object.DestroyImmediate(o);





        #endregion

        #region GlobalID

#if UNITY_EDITOR

        [System.Serializable]
        public struct GlobalID : System.IEquatable<GlobalID>
        {
            public Object GetObject() => GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalObjectId);
            public int GetObjectInstanceId() => GlobalObjectId.GlobalObjectIdentifierToInstanceIDSlow(globalObjectId);


            public string guid => globalObjectId.assetGUID.ToString();
            public ulong fileId => globalObjectId.targetObjectId;

            public bool isNull => globalObjectId.identifierType == 0;
            public bool isAsset => globalObjectId.identifierType == 1;
            public bool isSceneObject => globalObjectId.identifierType == 2;

            public GlobalObjectId globalObjectId => _globalObjectId.Equals(default) && GlobalObjectId.TryParse(globalObjectIdString, out var r) ? _globalObjectId = r : _globalObjectId;
            public GlobalObjectId _globalObjectId;

            public GlobalID(Object o) => globalObjectIdString = (_globalObjectId = GlobalObjectId.GetGlobalObjectIdSlow(o)).ToString();
            public GlobalID(string s) => globalObjectIdString = GlobalObjectId.TryParse(s, out _globalObjectId) ? s : s;

            public string globalObjectIdString;



            public bool Equals(GlobalID other) => this.globalObjectIdString.Equals(other.globalObjectIdString);

            public static bool operator ==(GlobalID a, GlobalID b) => a.Equals(b);
            public static bool operator !=(GlobalID a, GlobalID b) => !a.Equals(b);

            public override bool Equals(object other) => other is GlobalID otherglobalID && this.Equals(otherglobalID);
            public override int GetHashCode() => globalObjectIdString == null ? 0 : globalObjectIdString.GetHashCode();


            public override string ToString() => globalObjectIdString;

        }

        public static GlobalID GetGlobalID(this Object o) => new GlobalID(o);

        public static int[] GetObjectInstanceIds(this IEnumerable<GlobalID> globalIDs)
        {
            var goids = globalIDs.Select(r => r.globalObjectId).ToArray();

            var iids = new int[goids.Length];

            GlobalObjectId.GlobalObjectIdentifiersToInstanceIDsSlow(goids, iids);

            return iids;
        }


#endif




        #endregion

        #region Paths


        public static string GetParentPath(this string path) => path.Substring(0, path.LastIndexOf('/'));
        public static bool HasParentPath(this string path) => path.Contains('/') && path.GetParentPath() != "";

        public static string ToGlobalPath(this string localPath) => Application.dataPath + "/" + localPath.Substring(0, localPath.Length - 1);
        public static string ToLocalPath(this string globalPath) => "Assets" + globalPath.Remove(Application.dataPath);



        public static string CombinePath(this string p, string p2) => Path.Combine(p, p2);

        public static bool IsSubpathOf(this string path, string of) => path.StartsWith(of + "/") || of == "";

        public static string GetDirectory(this string pathOrDirectory)
        {
            var directory = pathOrDirectory.Contains('.') ? pathOrDirectory.Substring(0, pathOrDirectory.LastIndexOf('/')) : pathOrDirectory;

            if (directory.Contains('.'))
                directory = directory.Substring(0, directory.LastIndexOf('/'));

            return directory;

        }

        public static bool DirectoryExists(this string pathOrDirectory) => Directory.Exists(pathOrDirectory.GetDirectory());

        public static string EnsureDirExists(this string pathOrDirectory) // todo to EnsureDirectoryExists
        {
            var directory = pathOrDirectory.GetDirectory();

            if (directory.HasParentPath() && !Directory.Exists(directory.GetParentPath()))
                EnsureDirExists(directory.GetParentPath());

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return pathOrDirectory;

        }



        public static string ClearDir(this string dir)
        {
            if (!Directory.Exists(dir)) return dir;

            var diri = new DirectoryInfo(dir);
            foreach (var r in diri.EnumerateFiles()) r.Delete();
            foreach (var r in diri.EnumerateDirectories()) r.Delete(true);

            return dir;
        }






#if UNITY_EDITOR

        public static string EnsurePathIsUnique(this string path)
        {
            if (!path.DirectoryExists()) return path;

            var s = AssetDatabase.GenerateUniqueAssetPath(path); // returns empty if parent dir doesnt exist 

            return s == "" ? path : s;

        }

        public static void EnsureDirExistsAndRevealInFinder(string dir)
        {
            EnsureDirExists(dir);
            UnityEditor.EditorUtility.OpenWithDefaultApp(dir);
        }

#endif



        #endregion

        #region AssetDatabase

#if UNITY_EDITOR

        public static AssetImporter GetImporter(this Object t) => AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(t));

        public static string ToPath(this string guid) => AssetDatabase.GUIDToAssetPath(guid); // returns empty string if not found
        public static List<string> ToPaths(this IEnumerable<string> guids) => guids.Select(r => r.ToPath()).ToList();

        public static string GetFilename(this string path, bool withExtension = false) => withExtension ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path); // prev GetName
        public static string GetExtension(this string path) => Path.GetExtension(path);


        public static string ToGuid(this string pathInProject) => AssetDatabase.AssetPathToGUID(pathInProject);
        public static List<string> ToGuids(this IEnumerable<string> pathsInProject) => pathsInProject.Select(r => r.ToGuid()).ToList();

        public static string GetPath(this Object o) => AssetDatabase.GetAssetPath(o);
        public static string GetGuid(this Object o) => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));

        public static string GetScriptPath(string scriptName) => AssetDatabase.FindAssets("t: script " + scriptName, null).FirstOrDefault()?.ToPath() ?? "scirpt not found";





        // toremove
        public static Object LoadGuid(this string guid) => AssetDatabase.LoadAssetAtPath(guid.ToPath(), typeof(Object));
        public static T LoadGuid<T>(this string guid) where T : Object => AssetDatabase.LoadAssetAtPath<T>(guid.ToPath());




        public static List<string> FindAllAssetsOfType_guids(Type type) => AssetDatabase.FindAssets("t:" + type.Name).ToList();
        public static List<string> FindAllAssetsOfType_guids(Type type, string path) => AssetDatabase.FindAssets("t:" + type.Name, new[] { path }).ToList();
        public static List<T> FindAllAssetsOfType<T>() where T : Object => FindAllAssetsOfType_guids(typeof(T)).Select(r => (T)r.LoadGuid()).ToList();
        public static List<T> FindAllAssetsOfType<T>(string path) where T : Object => FindAllAssetsOfType_guids(typeof(T), path).Select(r => (T)r.LoadGuid()).ToList();

        public static T Reimport<T>(this T t) where T : Object { AssetDatabase.ImportAsset(t.GetPath(), ImportAssetOptions.ForceUpdate); return t; }

#endif





        #endregion

        #region Files

#if UNITY_EDITOR

        public static void Hide(string path)
        {
            if (IsHidden(path)) return;

            if (File.Exists(path))
                File.Move(path, path + "~");


            path += ".meta";
            if (File.Exists(path))
                File.Move(path, path + "~");
        }
        public static void Unhide(string path)
        {
            if (!IsHidden(path)) return;
            if (path.EndsWith("~")) path = path.Substring(0, path.Length - 1);

            if (File.Exists(path + "~"))
                File.Move(path + "~", path);

            path += ".meta";
            if (File.Exists(path + "~"))
                File.Move(path + "~", path);
        }
        public static bool IsHidden(this string path) => path.EndsWith("~") || File.Exists(path + "~");

        public static void CopyDirectoryDeep(string sourcePath, string destinationPath)
        {
            CopyDirectoryRecursively(sourcePath, destinationPath);

            var metas = GetFilesRecursively(destinationPath, (f) => f.EndsWith(".meta"));
            var guidTable = new List<(string originalGuid, string newGuid)>();

            foreach (string meta in metas)
            {
                StreamReader file = new StreamReader(meta);
                file.ReadLine();
                string guidLine = file.ReadLine();
                file.Close();
                string originalGuid = guidLine.Substring(6, guidLine.Length - 6);
                string newGuid = GUID.Generate().ToString().Replace("-", "");
                guidTable.Add((originalGuid, newGuid));
            }

            var allFiles = GetFilesRecursively(destinationPath);

            foreach (string fileToModify in allFiles)
            {
                string content = File.ReadAllText(fileToModify);

                foreach (var guidPair in guidTable)
                {
                    content = content.Replace(guidPair.originalGuid, guidPair.newGuid);
                }

                File.WriteAllText(fileToModify, content);
            }

            AssetDatabase.Refresh();
        }

        private static void CopyDirectoryRecursively(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                CopyDirectoryRecursively(subdir.FullName, temppath);
            }
        }

        private static List<string> GetFilesRecursively(string path, System.Func<string, bool> criteria = null, List<string> files = null)
        {
            if (files == null)
            {
                files = new List<string>();
            }

            files.AddRange(Directory.GetFiles(path).Where(f => criteria == null || criteria(f)));

            foreach (string directory in Directory.GetDirectories(path))
            {
                GetFilesRecursively(directory, criteria, files);
            }

            return files;
        }

#endif




        #endregion

        #region Serialization

        [System.Serializable]
        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
        {
            public List<TKey> keys = new List<TKey>();
            public List<TValue> values = new List<TValue>();

            public void OnBeforeSerialize()
            {
                keys.Clear();
                values.Clear();

                foreach (KeyValuePair<TKey, TValue> kvp in this)
                {
                    keys.Add(kvp.Key);
                    values.Add(kvp.Value);
                }

            }
            public void OnAfterDeserialize()
            {
                this.Clear();

                for (int i = 0; i < keys.Count; i++)
                    this[keys[i]] = values[i];

            }

        }


        #endregion

        #region Editor

#if UNITY_EDITOR

        public static void ToggleDefineDisabledInScript(Type scriptType)
        {
            var path = GetScriptPath(scriptType.Name);

            var lines = File.ReadAllLines(path);
            if (lines.First().StartsWith("#define DISABLED"))
                File.WriteAllLines(path, lines.Skip(1));
            else
                File.WriteAllLines(path, lines.Prepend("#define DISABLED    // this line was added by VUtils.ToggleDefineDisabledInScript"));

            AssetDatabase.ImportAsset(path);
        }
        public static bool ScriptHasDefineDisabled(Type scriptType) => File.ReadLines(GetScriptPath(scriptType.Name)).First().StartsWith("#define DISABLED");
        public static void SetDefineDisabledInScript(Type scriptType, bool defineDisabled)
        {
            if (ScriptHasDefineDisabled(scriptType) != defineDisabled)
                ToggleDefineDisabledInScript(scriptType);

        }

        public static int GetProjectId() => Application.dataPath.GetHashCode();

        public static void PingObject(Object o, bool select = false, bool focusProjectWindow = true)
        {
            if (select)
            {
                Selection.activeObject = null;
                Selection.activeObject = o;
            }
            if (focusProjectWindow) EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(o);

        }
        public static void PingObject(string guid, bool select = false, bool focusProjectWindow = true) => PingObject(AssetDatabase.LoadAssetAtPath<Object>(guid.ToPath()));


        public static void OpenFolder(string path)
        {
            var folder = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

            var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            var w = (EditorWindow)t.GetField("s_LastInteractedProjectBrowser").GetValue(null);

            var m_ListAreaState = t.GetField("m_ListAreaState", maxBindingFlags).GetValue(w);

            m_ListAreaState.GetType().GetField("m_SelectedInstanceIDs").SetValue(m_ListAreaState, new List<int> { folder.GetInstanceID() });

            t.GetMethod("OpenSelectedFolders", maxBindingFlags).Invoke(null, null);

        }

        public static void Dirty(this Object o) => UnityEditor.EditorUtility.SetDirty(o);
        public static void RecordUndo(this Object o) => Undo.RecordObject(o, "");
#if UNITY_2021_1_OR_NEWER
        public static void Save(this Object o) => AssetDatabase.SaveAssetIfDirty(o);
#else
        public static void Save(this Object o) { }
#endif


        public static EditorWindow OpenObjectPicker<T>(Object obj = null, bool allowSceneObjects = false, string searchFilter = "", int controlID = 0) where T : Object
        {
            EditorGUIUtility.ShowObjectPicker<T>(obj, allowSceneObjects, searchFilter, controlID);

            return Resources.FindObjectsOfTypeAll(typeof(Editor).Assembly.GetType("UnityEditor.ObjectSelector")).FirstOrDefault() as EditorWindow;

        }
        public static EditorWindow OpenColorPicker(System.Action<Color> colorChangedCallback, Color color, bool showAlpha = true, bool hdr = false)
        {
            typeof(Editor).Assembly.GetType("UnityEditor.ColorPicker").InvokeMethod("Show", colorChangedCallback, color, showAlpha, hdr);

            return typeof(Editor).Assembly.GetType("UnityEditor.ColorPicker").GetPropertyValue<EditorWindow>("instance");

        }

        public static void MoveTo(this EditorWindow window, Vector2 position, bool ensureFitsOnScreen = true)
        {
            if (!ensureFitsOnScreen) { window.position = window.position.SetPos(position); return; }

            var windowRect = window.position;
            var unityWindowRect = EditorGUIUtility.GetMainWindowPosition();

            position.x = position.x.Max(unityWindowRect.position.x);
            position.y = position.y.Max(unityWindowRect.position.y);

            position.x = position.x.Min(unityWindowRect.xMax - windowRect.width);
            position.y = position.y.Min(unityWindowRect.yMax - windowRect.height);

            window.position = windowRect.SetPos(position);

        }



        public static void RemoveEditorErrors() => removeEditorErrorsMethod.Invoke(null, new object[] { 1 });
        static MethodInfo removeEditorErrorsMethod = System.AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(r => r.GetName().ToString().Contains("UnityEditor.CoreModule")).GetTypes().First(r => r.Name.Contains("LogEntry")).GetMethod("RemoveLogEntriesByMode", BindingFlags.Static | BindingFlags.NonPublic);


#endif





        #endregion

    }

    public static class VGUI
    {
        #region Controls

        public static T Field<T>(Rect rect, string name, T cur)
        {
            if (typeof(Object).IsAssignableFrom(typeof(T)))
                return (T)(object)EditorGUI.ObjectField(rect, name, cur as Object, typeof(T), true);

            if (typeof(T) == typeof(float))
                return (T)(object)EditorGUI.FloatField(rect, name, (float)(object)cur);

            if (typeof(T) == typeof(int))
                return (T)(object)EditorGUI.IntField(rect, name, (int)(object)cur);

            if (typeof(T) == typeof(bool))
                return (T)(object)EditorGUI.Toggle(rect, name, (bool)(object)cur);

            if (typeof(T) == typeof(string))
                return (T)(object)EditorGUI.TextField(rect, name, (string)(object)cur);

            return default;
        }
        public static T Field<T>(string name, T cur) => Field(ExpandWidthLabelRect(), name, cur);
        public static void Field<T>(Rect rect, string name, ref T cur) => cur = Field(rect, name, cur);
        public static void Field<T>(string name, ref T cur) => cur = Field(name, cur);

        public static T Slider<T>(Rect rect, string name, T cur, T min, T max)
        {
            if (typeof(T) == typeof(float))
                return (T)(object)EditorGUI.Slider(rect, name, (float)(object)cur, (float)(object)min, (float)(object)max);

            if (typeof(T) == typeof(int))
                return (T)(object)EditorGUI.IntSlider(rect, name, (int)(object)cur, (int)(object)min, (int)(object)max);

            return default;
        }
        public static T Slider<T>(string name, T cur, T min, T max) => Slider(ExpandWidthLabelRect(), name, cur, min, max);
        public static void Slider<T>(Rect rect, string name, ref T cur, T min, T max) => cur = Slider(rect, name, cur, min, max);
        public static void Slider<T>(string name, ref T cur, T min, T max) => cur = Slider(name, cur, min, max);



        public static bool _ResetFieldButton(Rect rect, bool isObjectField = false)
        {
            var prev = GUI.color;
            GUI.color = Color.clear;
            var r = GUI.Button(rect.SetWidthFromRight(20).MoveX(isObjectField ? -18 : 0), "");
            GUI.color = prev;
            return r;
        }
        public static void _DrawResettableFieldCrossIcon(Rect rect, bool isObjectField = false, float brightness = .36f)
        {
            var iconRect = rect.SetWidthFromRight(20).SetSizeFromMid(15).MoveX(isObjectField ? -18 : 0).MoveY(.5f);

            if (iconRect.Resize(-3).IsHovered())
                brightness = isDarkTheme ? .8f : .65f;

            if (isObjectField)
            {
                var fieldBg = isDarkTheme ? Greyscale(.152f) : Greyscale(.93f);
                iconRect.MoveX(-2).SetWidth(19).Draw(fieldBg);
            }

            SetGUIColor(Greyscale(brightness));
            GUI.Label(iconRect, EditorGUIUtility.IconContent("CrossIcon"));
            ResetGUIColor();

        }

        public static T ResettableField<T>(Rect rect, string name, T cur, T resetTo = default)
        {
            var isObjectField = typeof(Object).IsAssignableFrom(typeof(T));
            var reset = _ResetFieldButton(rect, isObjectField);

            cur = Field(rect, name, cur);

            if (!object.Equals(cur, resetTo))
                _DrawResettableFieldCrossIcon(rect, isObjectField);

            return reset ? resetTo : cur;
        }
        public static T ResettableField<T>(string name, T cur, T resetTo = default) => ResettableField(ExpandWidthLabelRect(), name, cur, resetTo);
        public static void ResettableField<T>(Rect rect, string name, ref T cur, T resetTo = default) => cur = ResettableField(rect, name, cur, resetTo);
        public static void ResettableField<T>(string name, ref T cur, T resetTo = default) => cur = ResettableField(name, cur, resetTo);

        public static T ResettableSlider<T>(Rect rect, string name, T cur, T min, T max, T resetTo = default)
        {
            var reset = _ResetFieldButton(lastRect);

            cur = Slider(rect, name, cur, min, max);

            if (!object.Equals(cur, resetTo))
                _DrawResettableFieldCrossIcon(lastRect);

            return reset ? resetTo : cur;
        }
        public static T ResettableSlider<T>(string name, T cur, T min, T max, T resetTo = default) => ResettableSlider(ExpandWidthLabelRect(), name, cur, min, max, resetTo);
        public static void ResettableSlider<T>(Rect rect, string name, ref T cur, T min, T max, T resetTo = default) => cur = ResettableSlider(rect, name, cur, min, max, resetTo);
        public static void ResettableSlider<T>(string name, ref T cur, T min, T max, T resetTo = default) => cur = ResettableSlider(name, cur, min, max, resetTo);



        // public static int ResettableFieldWithPlusMinsButtons(Rect rect, string name, int cur, int min, int max, int resetTo = 0, int increment = 1)
        // {
        //     var buttonWidth = 24;

        //     cur = ResettableField(rect.SetWidth(rect.width), name, cur, resetTo);
        //     cur = Mathf.Clamp(cur, min, max);


        //     var prev = GUI.enabled;
        //     GUI.skin.label.alignment = TextAnchor.MiddleCenter;

        //     GUI.enabled = cur != max;
        //     var rPlus = rect.MoveX(EditorGUIUtility.labelWidth).SetWidth(buttonWidth).Resize(-.5f);
        //     if (GUI.Button(rPlus, ""))
        //         cur += increment;
        //     GUI.Label(rPlus.Resize(2), EditorGUIUtility.IconContent("Toolbar Plus"));

        //     GUI.enabled = cur != min;
        //     var rMinus = rPlus.MoveX() SetWidth(buttonWidth).MoveX(-buttonWidth + 1).Resize(-.5f);
        //     if (GUI.Button(rMinus, ""))
        //         cur -= increment;
        //     GUI.Label(rMinus.Resize(2), EditorGUIUtility.IconContent("Toolbar Minus"));

        //     GUI.enabled = prev;
        //     GUI.skin.label.alignment = TextAnchor.MiddleLeft;



        //     return cur;
        // }
        public static int ResettableFieldWithPlusMinsButtons(Rect rect, string name, int cur, int min, int max, int resetTo = 0, int increment = 1)
        {
            var buttonWidth = 24;

            cur = ResettableField(rect.SetWidth(rect.width - buttonWidth * 2 - 4), name, cur, resetTo);
            cur = Mathf.Clamp(cur, min, max);

            var prev = GUI.enabled;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            GUI.enabled = cur != max;
            var rPlus = rect.SetWidthFromRight(buttonWidth).Resize(-.5f);
            if (GUI.Button(rPlus, ""))
                cur += increment;
            GUI.Label(rPlus.Resize(2), EditorGUIUtility.IconContent("Toolbar Plus"));

            GUI.enabled = cur != min;
            var rMinus = rect.SetWidthFromRight(buttonWidth).MoveX(-buttonWidth + 1).Resize(-.5f);
            if (GUI.Button(rMinus, ""))
                cur -= increment;
            GUI.Label(rMinus.Resize(2), EditorGUIUtility.IconContent("Toolbar Minus"));

            GUI.enabled = prev;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;



            return cur;
        }
        public static int ResettableFieldWithPlusMinsButtons(string name, int cur, int min, int max, int resetTo = 0, int increment = 1) => ResettableFieldWithPlusMinsButtons(ExpandWidthLabelRect(), name, cur, min, max, resetTo, increment);
        public static int ResettableFieldWithPlusMinsButtons(Rect rect, string name, ref int cur, int min, int max, int resetTo = 0, int increment = 1) => cur = ResettableFieldWithPlusMinsButtons(rect, name, cur, min, max, resetTo, increment);
        public static int ResettableFieldWithPlusMinsButtons(string name, ref int cur, int min, int max, int resetTo = 0, int increment = 1) => cur = ResettableFieldWithPlusMinsButtons(name, cur, min, max, resetTo, increment);

        public static int FieldWithPlusMinsButtons(Rect rect, string name, int cur, int min, int max, int increment = 1) => ResettableFieldWithPlusMinsButtons(rect, name, cur, min, max, cur, increment);
        public static int FieldWithPlusMinsButtons(string name, int cur, int min, int max, int increment = 1) => FieldWithPlusMinsButtons(ExpandWidthLabelRect(), name, cur, min, max, increment);
        public static int FieldWithPlusMinsButtons(Rect rect, string name, ref int cur, int min, int max, int increment = 1) => cur = FieldWithPlusMinsButtons(rect, name, cur, min, max, increment);
        public static int FieldWithPlusMinsButtons(string name, ref int cur, int min, int max, int increment = 1) => cur = FieldWithPlusMinsButtons(name, cur, min, max, increment);


        public static bool OnOffButton(Rect rect, string name, bool val)
        {
            var r = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 12321;
            EditorGUI.PrefixLabel(rect, new GUIContent(name));
            EditorGUIUtility.labelWidth = r;

            var rr = GUI.backgroundColor;
            GUI.backgroundColor = val ? GUIColors.pressedButtonBackground : rr;
            if (GUI.Button(rect.SetWidthFromRight(50).Resize(-1), val ? "On\u2009\u2009" : "Off"))
                val = !val;
            GUI.backgroundColor = rr;


            return val;
        }
        public static bool OnOffButton(string name, bool val) => OnOffButton(ExpandWidthLabelRect(), name, val);
        public static void OnOffButton(Rect rect, string name, ref bool val) => val = OnOffButton(rect, name, val);
        public static void OnOffButton(string name, ref bool val) => val = OnOffButton(ExpandWidthLabelRect(), name, val);


        public static bool Toggle(Rect rect, string name, bool val)
        {
            val = EditorGUI.Toggle(rect, name, val);

            var r = GUI.color;
            GUI.color = Color.clear;
            if (GUI.Button(lastRect, ""))
                val = !val;
            GUI.color = Color.white;

            return val;
        }
        public static bool Toggle(string name, bool val) => Toggle(ExpandWidthLabelRect(), name, val);
        public static void Toggle(Rect rect, string name, ref bool val) => val = Toggle(rect, name, val);
        public static void Toggle(string name, ref bool val) => val = Toggle(ExpandWidthLabelRect(), name, val);



        public static bool LeftToggle(Rect rect, string name, bool val, float offset = 0)
        {
            rect = rect.MoveX(offset);
            val = EditorGUI.Toggle(rect, val);

            var lr = rect.MoveX(19);
            EditorGUI.LabelField(lr, name);


            return val;
        }
        public static bool LeftToggle(string name, bool val, float offset = 0) => LeftToggle(ExpandWidthLabelRect(), name, val, offset);
        public static void LeftToggle(Rect rect, string name, ref bool val, float offset = 0) => val = LeftToggle(rect, name, val, offset);
        public static void LeftToggle(string name, ref bool val, float offset = 0) => val = LeftToggle(ExpandWidthLabelRect(), name, val, offset);



        public static Color SmallColorField(Rect rect, string name, Color val, bool showAlpha = true, bool hdr = false)
        {
            EditorGUI.PrefixLabel(rect, new GUIContent(name));


            var colorRect = rect.SetWidthFromRight(50).MoveX(2);


            val = EditorGUI.ColorField(colorRect.Resize(2), new GUIContent(""), val, false, showAlpha, hdr);


            var outlineColor = Greyscale(colorRect.IsHovered() ? .5f : .1f);

            var colNoAlpha = new Color(val.r, val.g, val.b, 1);

            colorRect.SetWidthFromMid(colorRect.width - 2).Draw(outlineColor);
            colorRect.SetHeightFromMid(colorRect.height - 2).Draw(outlineColor);

            colorRect.Resize(2).SetWidthFromMid(colorRect.width - 2).Draw(colNoAlpha);
            colorRect.Resize(2).SetHeightFromMid(colorRect.height - 2).Draw(colNoAlpha);


            if (showAlpha)
            {
                var alphaRect = colorRect.SetHeightFromBottom(1).AddWidth(-4).MoveX(2).MoveY(-1);

                alphaRect.Draw();
                alphaRect.SetWidth(alphaRect.width * val.a).Draw(Color.white);

            }


            return val;
        }
        public static Color SmallColorField(string name, Color val, bool showAlpha = true, bool hdr = false) => SmallColorField(ExpandWidthLabelRect().MoveX(-1), name, val, showAlpha, hdr);
        public static void SmallColorField(Rect rect, string name, ref Color val, bool showAlpha = true, bool hdr = false) => val = SmallColorField(rect, name, val, showAlpha, hdr);
        public static void SmallColorField(string name, ref Color val, bool showAlpha = true, bool hdr = false) => val = SmallColorField(ExpandWidthLabelRect(), name, val, showAlpha, hdr);




        public static void ObjectFieldWidhoutPicker<T>(Rect rect, string name, T val) where T : Object
        {
            var id = GUIUtility.GetControlID("s_ObjectFieldHash".GetHashCode(), FocusType.Keyboard, rect);

            void draw()
            {
                if (!curEvent.isRepaint) return;


                EditorGUI.PrefixLabel(rect, new GUIContent(name));


                var fieldRect = rect.SetWidthFromRight(rect.width - EditorGUIUtility.labelWidth);

                var prev = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize(Vector2.one * 12);

                var style = new GUIStyle(EditorStyles.textField);
                style.imagePosition = ImagePosition.ImageLeft;
                style.Draw(fieldRect, EditorGUIUtility.ObjectContent(val, typeof(T)), id, DragAndDrop.activeControlID == id, fieldRect.IsHovered());

                EditorGUIUtility.SetIconSize(prev);

            }
            void click()
            {
                if (!curEvent.isMouseDown || !rect.IsHovered() || curEvent.mouseButton != 0) return;


                EditorGUIUtility.editingTextField = false;

                var obj = val as Object;
                var component = val as Component;

                if (component)
                    obj = component.gameObject;

                if (curEvent.clickCount == 1)
                {
                    GUIUtility.keyboardControl = id;

                    EditorGUIUtility.PingObject(obj);
                    curEvent.Use();
                }

                if (curEvent.clickCount == 2 || (component && curEvent.clickCount == 1))
                {
                    if (obj)
                    {
                        AssetDatabase.OpenAsset(obj);
                        GUIUtility.ExitGUI();
                    }
                    curEvent.Use();
                }
            }


            draw();
            click();


        }
        public static void ObjectFieldWidhoutPicker<T>(string name, T val) where T : Object => ObjectFieldWidhoutPicker(ExpandWidthLabelRect(), name, val);




        public static bool InvisibleButton(Rect rect)
        {
            var c = GUI.color;
            var e = GUI.enabled;
            GUI.color = Color.clear;
            GUI.enabled = true;
            var r = GUI.Button(rect, "");
            GUI.color = c;
            GUI.enabled = e;

            return r;

        }







        public static bool Foldout(string name, bool val)
        {
            EditorGUILayout.PrefixLabel(name);
            val = EditorGUI.Foldout(lastRect, val, "");

            if (lastRect.IsHovered() && curEvent.isMouseDown)
                val = !val;

            return val;
        }

        public static bool ClearButtonText(Rect rect, string text, bool val = false, bool drawPressed = false)
        {
            var hovered = rect.IsHovered();

            if (hovered && curEvent.isMouseUp)
            {
                val = !val;
                curEvent.Use();
            }

            var col = Color.white;
            if (drawPressed && val) col *= new Color(.35f, .57f, 1f, 1f) * 1.25f;

            var r = GUI.skin.label.alignment;
            var rr = GUI.color;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.color = col;
            GUI.Label(rect, text);
            GUI.color = rr;
            GUI.skin.label.alignment = r;

            return val;
        }



        public static void ColorPickerButton(Rect rect, Color color, System.Action<Color> colorChangedCallback, bool showAlphaBar = false, Color displayTint = default, bool greyedOut = false)
        {
            if (displayTint == default)
                displayTint = Color.white;

            if (GUI.Button(rect, ""))
                OpenColorPicker(colorChangedCallback, color, true, false);

            var c = color; c.a = 1;

            rect.SetHeight(showAlphaBar ? rect.height - 2 : rect.height).Draw(c * displayTint * (greyedOut ? GUIColors.greyedOutTint : Color.white));

            if (showAlphaBar)
            {
                var rAlphaBar = rect.SetHeightFromBottom(2).MoveY(0);
                rAlphaBar.width *= color.a;
                rAlphaBar.Draw(Greyscale(1, .65f) * (greyedOut ? GUIColors.greyedOutTint : Color.white));
            }

            // rect.DrawOutline(Greyscale(.15f));
        }





        public static int ResolutionPopup(string name, int cur, int min = 256, int max = 4096)
        {
            var ints = Enumerable.Range((int)Mathf.Log(min, 2), (int)Mathf.Log(max, 2) - (int)Mathf.Log(min, 2) + 1).Select(r => (int)Mathf.Pow(2, r)).ToArray();
            return EditorGUILayout.IntPopup(name, cur, ints.Select(r => r.ToString()).ToArray(), ints);
        }
        public static int HeightmapResolutionPopup(string name, int cur, int min = 513, int max = 16385)
        {
            var ints = Enumerable.Range((int)Mathf.Log(min - 1, 2), (int)Mathf.Log(max - 1, 2) - (int)Mathf.Log(min - 1, 2) + 1).Select(r => (int)Mathf.Pow(2, r)).Select(r => r + 1).ToArray();
            return EditorGUILayout.IntPopup(name, cur, ints.Select(r => r.ToString()).ToArray(), ints);
        }

        public static T SelectorPopup<T>(Rect rect, string name, T selected, List<T> options, System.Func<T, string> nameGetter, int labelOffset = -1)
        {
            var names = options.Select(r => nameGetter(r)).ToList();
            var curName = nameGetter(selected);
            var ints = Enumerable.Range(0, options.Count);
            int r = 0;
            if (labelOffset < 0)
                r = EditorGUI.IntPopup(rect, name, names.IndexOf(curName), names.ToArray(), ints.ToArray());
            else
            {
                r = EditorGUI.IntPopup(rect, names.IndexOf(curName), names.ToArray(), ints.ToArray());
                GUI.Label(rect.MoveX(-name.GetLabelWidth() - labelOffset), name);
            }

            return options[r];
        }
        public static T SelectorPopup<T>(string name, T selected, List<T> options, System.Func<T, string> nameGetter) { GUILayout.Label("", GUILayout.ExpandWidth(true)); return SelectorPopup(lastRect, name, selected, options, nameGetter); }

        // todo make it a class 
        public static void SelectorWithIcons<T>(ref T selected, List<T> options, System.Func<T, string> nameGetter, System.Func<T, Texture> textureGetter, System.Func<T, Rect?> uvsGetter, ref Vector2 scrollPos,
            ref bool renaming, ref string renamingTempName, float buttonSize = 60, float iconBorder = 1, bool showNoneOption = false, bool dragndropPossible = false,
            System.Action onAdd = null, System.Action<Object> onDragndrop = null, System.Action onRenameEnded = null, System.Action onSelectedClicked = null, System.Action<T> onRightclicked = null, Material iconMaterial = null, bool showPlusButton = true,
            string noneOptionText = "None", Texture2D noneOptionTexture = null)
        {
            var renaming_ = renaming;
            var selected_ = selected;
            bool pressedSelect = false;

            var newNamerenamingTempName_ = renamingTempName;

            Rect nameRect(Rect lastRect) => lastRect.MoveY(lastRect.height).Resize(1).SetHeight(18);
            void drawSelectedBlue(Rect lastRect, string name)
            {
                var rname = nameRect(lastRect);

                var rselect = rname.SetWidth(Mathf.Min(rname.width, name.GetLabelWidth() + 6));
                rselect.x += (rname.width - rselect.width) / 2;
                rselect = rselect.Resize(2);
                rselect.Draw(GUI.skin.settings.selectionColor * .9f);
            }
            void drawName(Rect rname, string name)
            {
                if (name.GetLabelWidth() < buttonSize)
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUI.Label(rname, name);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            }

            void item(T t)
            {
                if (GUILayout.Button("", GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
                {
                    if (t.Equals(selected_)) onSelectedClicked?.Invoke();
                    else { selected_ = t; pressedSelect = true; }

                    if (curEvent.mouseButton == 1 && onRightclicked != null)
                        onRightclicked(t);
                }


                // if (iconMaterial) EditorGUI.DrawPreviewTexture(lastRect.Resize(iconBorder), textureGetter(t), iconMaterial);
                // else EditorGUI.DrawPreviewTexture(lastRect.Resize(iconBorder), textureGetter(t));
                var uvs = uvsGetter?.Invoke(t);
                if (uvs != null)
                    GUI.DrawTextureWithTexCoords(lastRect.Resize(iconBorder), textureGetter(t), uvs.GetValueOrDefault());
                else if (iconMaterial != null)
                    EditorGUI.DrawPreviewTexture(lastRect.Resize(iconBorder), textureGetter(t), iconMaterial);
                else
                    GUI.DrawTexture(lastRect.Resize(iconBorder), textureGetter(t));



                if (t.Equals(selected_)) drawSelectedBlue(lastRect, nameGetter(t));


                if (curEvent.isKeyDown && curEvent.keyCode == KeyCode.Return)
                    if (!renaming_) { newNamerenamingTempName_ = nameGetter(selected_); renaming_ = true; curEvent.Use(); }
                    else { onRenameEnded(); renaming_ = false; curEvent.Use(); }

                if (t.Equals(selected_) && renaming_)
                {
                    GUI.SetNextControlName("w");
                    GUI.FocusControl("w");
                    newNamerenamingTempName_ = GUI.TextField(nameRect(lastRect), newNamerenamingTempName_);
                }
                else
                    drawName(nameRect(lastRect), nameGetter(t));

            }
            void plus()
            {
                if (!showPlusButton) return;

                GUILayout.Label("", GUILayout.Height(buttonSize), GUILayout.Width(buttonSize));
                var rect = lastRect.Resize(buttonSize / 9).MoveX(-buttonSize / 9);

                if (GUI.Button(rect, EditorGUIUtility.IconContent("Toolbar Plus")))
                    onAdd();



                if (!dragndropPossible) return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                var mouseIn = rect.Contains(curEvent.mousePosition);

                if (mouseIn) EditorGUI.DrawRect(rect, Greyscale(.8f, .07f));



                if (!curEvent.isDragPerform || !mouseIn) return;

                DragAndDrop.AcceptDrag();

                onDragndrop(DragAndDrop.objectReferences[0]);
            }
            void none()
            {
                if (!showNoneOption) return;


                if (GUILayout.Button(noneOptionTexture, GUILayout.Height(buttonSize), GUILayout.Width(buttonSize)))
                    selected_ = default(T);




                // if (selected_.Equals(default(T))) drawSelectedBlue(lastRect, noneOptionText);
                if (selected_ == null) drawSelectedBlue(lastRect, noneOptionText);
                drawName(nameRect(lastRect), noneOptionText);
            }


            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUIStyle.none, GUIStyle.none);
            GUILayout.BeginHorizontal();


            none();

            foreach (var r in options)
            {
                item(r); Space(2);
            }

            plus();

            GUILayout.EndHorizontal();
            GUILayout.Space(24);
            EditorGUILayout.EndScrollView();


            renamingTempName = newNamerenamingTempName_;
            renaming = renaming_;

            if (pressedSelect) selected = selected_;
        }


        // prev Toggle
        public static bool ToggleLeftOld(string s, bool b, float offset = 6, float textOffset = -4, bool expandWidth = true)
        {
            bool washor = __hor;
            if (!washor) Horizontal();
            GUILayout.Space(offset);
            var r = EditorGUILayout.Toggle(b, GUILayout.Width(20));
            GUILayout.Space(EditorGUI.indentLevel * 16 + textOffset);
            GUILayout.Label(s, GUILayout.ExpandWidth(expandWidth));
            if (!washor) Horizontal();

            return r;
        }

        public static bool ToggleLeftOld(Rect rect, string s, bool b, float offset = 6, float textOffset = -4, bool expandWidth = true)
        {
            // bool washor = __hor;

            return EditorGUI.ToggleLeft(rect, s, b);

            // var r = EditorGUILayout.Toggle(b, GUILayout.Width(20));
            // GUILayout.Space(EditorGUI.indentLevel * 16 + textOffset);
            // GUILayout.Label(s, GUILayout.ExpandWidth(expandWidth));

            // return r;
        }







        public static float ExpSlider(Rect rect, string name, float val, float min, float max, bool showDecimal = true)
        {
            EditorGUI.PrefixLabel(rect, new GUIContent(name));

            var valWindowSize = 50f;


            var rVal = rect.SetWidthFromRight(valWindowSize);

            if (!showDecimal)
            {
                var i = Mathf.RoundToInt(val);
                var ii = EditorGUI.IntField(rVal, i);
                if (ii != i)
                    val = ii;
            }
            else
                val = EditorGUI.FloatField(rVal, val);


            var rSlider = rect.MoveX(-valWindowSize - 4).SetWidthFromRight(rect.width - EditorGUIUtility.labelWidth - valWindowSize - 6);
            var e = 2.71828;
            var k = (max - min) / (e - 1);
            var c0 = min - k;
            var t = System.Math.Log(((double)val - c0) / k);

            EditorGUI.BeginChangeCheck();
            var tSlider = GUI.HorizontalSlider(rSlider, (float)t, 0, 1);
            var newVal = (float)(c0 + k * System.Math.Pow(e, tSlider));


            return EditorGUI.EndChangeCheck() ? Mathf.Clamp(newVal, min, max) : val;
        }
        public static float ExpSlider(string name, float val, float min, float max, bool showDecimal = true) => ExpSlider(ExpandWidthLabelRect(), name, val, min, max, showDecimal);




        #endregion

        #region Colors

        public static class GUIColors
        {
            public static Color windowBackground => isDarkTheme ? Greyscale(.22f) : Greyscale(.78f); // prev backgroundCol
            public static Color pressedButtonBackground => isDarkTheme ? new Color(.48f, .76f, 1f, 1f) * 1.4f : new Color(.48f, .7f, 1f, 1f) * 1.2f; // prev pressedButtonCol
            public static Color greyedOutTint => Greyscale(.7f);
            public static Color selectedBackground => isDarkTheme ? new Color(.17f, .365f, .535f) : new Color(.2f, .375f, .555f) * 1.2f;
        }


        #endregion

        #region Shortcuts

        public static Rect lastRect => GUILayoutUtility.GetLastRect();

        public static bool isDarkTheme => EditorGUIUtility.isProSkin;

        public static float GetLabelWidth(this string s) => GUI.skin.label.CalcSize(new GUIContent(s)).x;
        public static float GetLabelWidth(this string s, int fotSize)
        {
            SetLabelFontSize(fotSize);

            var r = s.GetLabelWidth();

            ResetLabelStyle();

            return r;

        }
        public static float GetLabelWidth(this string s, bool isBold)
        {
            if (isBold)
                SetLabelBold();

            var r = s.GetLabelWidth();

            if (isBold)
                ResetLabelStyle();

            return r;

        }

        public static void SetGUIEnabled(bool enabled) { _prevGuiEnabled = GUI.enabled; GUI.enabled = enabled; }
        public static void ResetGUIEnabled() => GUI.enabled = _prevGuiEnabled;
        static bool _prevGuiEnabled = true;

        public static void SetLabelFontSize(int size) => GUI.skin.label.fontSize = size;
        public static void SetLabelBold() => GUI.skin.label.fontStyle = FontStyle.Bold;
        public static void SetLabelAlignmentCenter() => GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        public static void ResetLabelStyle()
        {
            GUI.skin.label.fontSize = 0;
            GUI.skin.label.fontStyle = FontStyle.Normal;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        }


        public static void SetGUIColor(Color c)
        {
            if (!_guiColorModified)
                _defaultGuiColor = GUI.color;

            _guiColorModified = true;

            GUI.color = _defaultGuiColor * c;

        }
        public static void ResetGUIColor()
        {
            GUI.color = _guiColorModified ? _defaultGuiColor : Color.white;

            _guiColorModified = false;

        }
        static bool _guiColorModified;
        static Color _defaultGuiColor;



        #endregion

        #region Events

        public struct WrappedEvent
        {
            public Event e;

            public bool isNull => e == null;
            public bool isRepaint => isNull ? default : e.type == EventType.Repaint;
            public bool isLayout => isNull ? default : e.type == EventType.Layout;
            public bool isUsed => isNull ? default : e.type == EventType.Used;
            public bool isMouseLeaveWindow => isNull ? default : e.type == EventType.MouseLeaveWindow;
            public bool isMouseEnterWindow => isNull ? default : e.type == EventType.MouseEnterWindow;
            public bool isContextClick => isNull ? default : e.type == EventType.ContextClick;

            public bool isKeyDown => isNull ? default : e.type == EventType.KeyDown;
            public bool isKeyUp => isNull ? default : e.type == EventType.KeyUp;
            public KeyCode keyCode => isNull ? default : e.keyCode;
            public char characted => isNull ? default : e.character;

            public bool isExecuteCommand => isNull ? default : e.type == EventType.ExecuteCommand;
            public string commandName => isNull ? default : e.commandName;

            public bool isMouse => isNull ? default : e.isMouse;
            public bool isMouseDown => isNull ? default : e.type == EventType.MouseDown;
            public bool isMouseUp => isNull ? default : e.type == EventType.MouseUp;
            public bool isMouseDrag => isNull ? default : e.type == EventType.MouseDrag;
            public bool isMouseMove => isNull ? default : e.type == EventType.MouseMove;
            public bool isScroll => isNull ? default : e.type == EventType.ScrollWheel;
            public int mouseButton => isNull ? default : e.button;
            public int clickCount => isNull ? default : e.clickCount;
            public Vector2 mousePosition => isNull ? default : e.mousePosition;
            public Vector2 mousePosition_screenSpace => isNull ? default : GUIUtility.GUIToScreenPoint(e.mousePosition);
            public Vector2 mouseDelta => isNull ? default : e.delta;

            public bool isDragUpdate => isNull ? default : e.type == EventType.DragUpdated;
            public bool isDragPerform => isNull ? default : e.type == EventType.DragPerform;
            public bool isDragExit => isNull ? default : e.type == EventType.DragExited;

            public EventModifiers modifiers => isNull ? default : e.modifiers;
            public bool holdingAnyModifierKey => modifiers != EventModifiers.None;

            public bool holdingAlt => isNull ? default : e.alt;
            public bool holdingShift => isNull ? default : e.shift;
            public bool holdingCtrl => isNull ? default : e.control;
            public bool holdingCmd => isNull ? default : e.command;
            public bool holdingCmdOrCtrl => isNull ? default : e.command || e.control;

            public bool holdingAltOnly => isNull ? default : e.modifiers == EventModifiers.Alt;        // in some sessions FunctionKey is always pressed?
            public bool holdingShiftOnly => isNull ? default : e.modifiers == EventModifiers.Shift;        // in some sessions FunctionKey is always pressed?
            public bool holdingCtrlOnly => isNull ? default : e.modifiers == EventModifiers.Control;
            public bool holdingCmdOnly => isNull ? default : e.modifiers == EventModifiers.Command;
            public bool holdingCmdOrCtrlOnly => isNull ? default : (e.modifiers == EventModifiers.Command || e.modifiers == EventModifiers.Control);

            public EventType type => e.type;

            public void Use() => e?.Use();


            public WrappedEvent(Event e) => this.e = e;

            public override string ToString() => e.ToString();

        }
        public static WrappedEvent Wrap(this Event e) => new WrappedEvent(e);
        public static WrappedEvent curEvent => (Event.current ?? _fi_s_Current.GetValue(null) as Event).Wrap(); // todo no reflection?
        static FieldInfo _fi_s_Current = typeof(Event).GetField("s_Current", maxBindingFlags);

        // public static Event e => Event.current;
        // public static bool ePresent => Event.current != null;
        // public static UnityEngine.EventType eType => ePresent ? e.type : UnityEngine.EventType.Ignore;
        // public static bool mouseDown(this Event e) => curEvent.isMouseDown && curEvent.mouseButton == 0;
        // public static bool mouseUp(this Event e) => curEvent.isMouseUp && curEvent.mouseButton == 0;
        // public static bool keyDown(this Event e) => curEvent.isKeyDown;
        // public static bool keyUp(this Event e) => eType == EventType.KeyUp;


        // public static bool holdingAlt => ePresent && (e.alt);
        // public static bool holdingCmd => ePresent && (e.command || e.control);
        // public static bool holdingShift => ePresent && (e.shift);






        #endregion

        #region Layout



        public static void BeginPanel(string title, float height, System.Action onClose = null, System.Action onApply = null)
        {

            void bg()
            {
                GUI.enabled = false;
                SetGUIColor(Greyscale(.75f));
                var r = ExpandWidthLabelRect(0).SetHeight(height);
                GUI.Button(r, "");
                GUI.Button(r, "");
                GUI.Button(r, "");
                GUI.Button(r, "");
                GUI.Button(r, "");
                GUI.Button(r, "");
                GUI.Button(r, "");
                GUI.Button(r, "");
                ResetGUIColor();
                GUI.enabled = true;
            }

            void layout()
            {

                GUILayout.BeginHorizontal();
                Space(7);
                EditorGUIUtility.labelWidth -= 7;
                GUILayout.BeginVertical();

            }

            void title_()
            {
                Space(5);
                EditorGUI.PrefixLabel(ExpandWidthLabelRect(), new GUIContent(title));
                // GUI.skin.label.fontStyle = FontStyle.Bold;
                // GUILayout.Label(title);

            }

            void buttons()
            {
                var rClose = lastRect.SetWidthFromRight(16).MoveX(1).MoveY(-1);

                // if()
                GUI.color = Greyscale(rClose.IsHovered() ? .9f : .6f);
                GUI.Label(rClose, EditorGUIUtility.IconContent("CrossIcon"));

                GUI.color = Color.clear;
                if (GUI.Button(rClose, ""))
                    onClose?.Invoke();


                var rApply = rClose.MoveX(-19).Resize(-1);

                GUI.color = Greyscale(rApply.IsHovered() ? .9f : .6f);
                GUI.Label(rApply, EditorGUIUtility.IconContent("check"));

                GUI.color = Color.clear;
                if (GUI.Button(rApply, ""))
                    onApply?.Invoke();


                GUI.color = Color.white;

            }



            bg();
            layout();
            title_();
            buttons();

            Space(5);

        }

        public static void EndPanel()
        {
            GUILayout.EndVertical();
            Space(7);
            EditorGUIUtility.labelWidth = 0;
            GUILayout.EndHorizontal();
        }



        public static void BeginIndent(float f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(f);
            GUILayout.BeginVertical();

            _indentLabelWidthStack.Push(EditorGUIUtility.labelWidth);

            EditorGUIUtility.labelWidth -= f;
        }

        public static void EndIndent(float f = 0)
        {
            GUILayout.EndVertical();
            GUILayout.Space(f);
            GUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = _indentLabelWidthStack.Pop();
        }
        static Stack<float> _indentLabelWidthStack = new Stack<float>();


        public static void Horizontal() { if (__hor) GUILayout.EndHorizontal(); else GUILayout.BeginHorizontal(); __hor = !__hor; }
        public static void Vertical() { if (__v) GUILayout.EndVertical(); else GUILayout.BeginVertical(); __v = !__v; }
        public static void Area(Rect r) { if (__a) GUILayout.EndArea(); else GUILayout.BeginArea(r); __a = !__a; }
        public static void Area() { if (__a) GUILayout.EndArea(); __a = !__a; }
        public static void ResetUIBools() { __a = __hor = __v = false; _prevGuiEnabled = true; }
        static bool __hor, __a, __v;

        #endregion

        #region Drawing

        public static Rect Draw(this Rect r) { EditorGUI.DrawRect(r, Color.black); return r; }
        public static Rect Draw(this Rect r, Color c) { EditorGUI.DrawRect(r, c); return r; }

        public static Rect DrawOutline(this Rect r) => r.DrawOutline(Color.black);
        public static Rect DrawOutline(this Rect r, Color c) { OutlineRect(r, c, false, 1); return r; }

        public static void OutlineRect(Rect rect, Color col, bool greyedOut = false, int px = 1)
        {
            bool offset = false;

            int f = px;
            Rect r;
            //
            Color tint = greyedOut ? Color.white * .74f : Color.white;

            r = rect;
            r.height = f;

            if (offset)
            {
                r.x += 1;
                r.width -= 2;
                r.y += 1;
            }

            EditorGUI.DrawRect(r, col * tint);

            r = rect;
            r.width = f;

            if (offset)
            {
                r.y += 1;
                r.height -= 2;
                r.x += 1;
            }

            EditorGUI.DrawRect(r, col * tint);

            r = rect;
            r.y += r.height;
            r.height = f;
            r.y -= r.height;

            if (offset)
            {
                r.x += 1;
                r.width -= 2;
                r.y -= 1;
            }

            EditorGUI.DrawRect(r, col * tint);

            r = rect;
            r.x += r.width;
            r.width = f;
            r.x -= r.width;

            if (offset)
            {
                r.y += 1;
                r.height -= 2;
                r.x -= 1;
            }

            EditorGUI.DrawRect(r, col * tint);
        }

        public static Rect DrawWithRoundedCorners(this Rect rect, Color color, int cornerRadius)
        {
            if (!curEvent.isRepaint) return rect;

            cornerRadius = cornerRadius.Min((rect.height / 2).FloorToInt()).Min((rect.width / 2).FloorToInt());

            GUIStyle style;

            void getStyle()
            {
                if (_roundedStylesByCornerRadius.TryGetValue(cornerRadius, out style)) return;

                var pixelsPerPoint = 2;

                var res = cornerRadius * 2 * pixelsPerPoint;
                var pixels = new Color[res * res];

                var white = Greyscale(1, 1);
                var clear = Greyscale(1, 0);
                var halfRes = res / 2;

                for (int x = 0; x < res; x++)
                    for (int y = 0; y < res; y++)
                    {
                        var sqrMagnitude = (new Vector2(x - halfRes + .5f, y - halfRes + .5f)).sqrMagnitude;
                        pixels[x + y * res] = sqrMagnitude <= halfRes * halfRes ? white : clear;
                    }

                var texture = new Texture2D(res, res);
                texture.SetPropertyValue("pixelsPerPoint", pixelsPerPoint);
                texture.hideFlags = HideFlags.DontSave;
                texture.SetPixels(pixels);
                texture.Apply();



                style = new GUIStyle();
                style.normal.background = texture;
                style.alignment = TextAnchor.MiddleCenter;
                style.border = new RectOffset(cornerRadius, cornerRadius, cornerRadius, cornerRadius);


                _roundedStylesByCornerRadius[cornerRadius] = style;

            }
            void draw()
            {
                SetGUIColor(color);

                style.Draw(rect, false, false, false, false);

                ResetGUIColor();

            }

            getStyle();
            draw();

            return rect;

        }
        public static Rect DrawWithRoundedCorners(this Rect rect, Color color, float cornerRadius) => rect.DrawWithRoundedCorners(color, cornerRadius.RoundToInt());
        static Dictionary<int, GUIStyle> _roundedStylesByCornerRadius = new Dictionary<int, GUIStyle>();


        public static Rect DrawBlurred(this Rect rect, Color color, int blurRadius)
        {
            if (!curEvent.isRepaint) return rect;

            var pixelsPerPoint = .5f;
            // var pixelsPerPoint = 1f;

            var blurRadiusScaled = (blurRadius * pixelsPerPoint).RoundToInt().Max(1).Min(123);

            var croppedRectWidth = (rect.width * pixelsPerPoint).RoundToInt().Min(blurRadiusScaled * 2);
            var croppedRectHeight = (rect.height * pixelsPerPoint).RoundToInt().Min(blurRadiusScaled * 2);

            var textureWidth = croppedRectWidth + blurRadiusScaled * 2;
            var textureHeight = croppedRectHeight + blurRadiusScaled * 2;

            GUIStyle style;

            void getStyle()
            {
                if (_blurredStylesByTextureSize.TryGetValue((textureWidth, textureHeight), out style)) return;

                // VDebug.LogStart(blurRadius + "");

                var pixels = new Color[textureWidth * textureHeight];
                var kernel = new GaussianKernel(false, blurRadiusScaled).Array2d();

                for (int x = 0; x < textureWidth; x++)
                    for (int y = 0; y < textureHeight; y++)
                    {
                        var sum = 0f;

                        for (int xSample = (x - blurRadiusScaled).Max(blurRadiusScaled); xSample <= (x + blurRadiusScaled).Min(textureWidth - 1 - blurRadiusScaled); xSample++)
                            for (int ySample = (y - blurRadiusScaled).Max(blurRadiusScaled); ySample <= (y + blurRadiusScaled).Min(textureHeight - 1 - blurRadiusScaled); ySample++)
                                sum += kernel[blurRadiusScaled + xSample - x, blurRadiusScaled + ySample - y];

                        pixels[x + y * textureWidth] = Greyscale(1, sum);

                    }

                var texture = new Texture2D(textureWidth, textureHeight);
                texture.SetPropertyValue("pixelsPerPoint", pixelsPerPoint);
                texture.hideFlags = HideFlags.DontSave;
                texture.SetPixels(pixels);
                texture.Apply();


                style = new GUIStyle();
                style.normal.background = texture;
                style.alignment = TextAnchor.MiddleCenter;

                var borderX = ((textureWidth / 2f - 1) / pixelsPerPoint).FloorToInt();
                var borderY = ((textureHeight / 2f - 1) / pixelsPerPoint).FloorToInt();
                style.border = new RectOffset(borderX, borderX, borderY, borderY);

                _blurredStylesByTextureSize[(textureWidth, textureHeight)] = style;

                // VDebug.LogFinish();

            }
            void draw()
            {
                SetGUIColor(color);

                style.Draw(rect.SetSizeFromMid(rect.width + blurRadius * 2, rect.height + blurRadius * 2), false, false, false, false);

                ResetGUIColor();

            }

            getStyle();
            draw();

            return rect;

        }
        public static Rect DrawBlurred(this Rect rect, Color color, float blurRadius) => rect.DrawBlurred(color, blurRadius.RoundToInt());
        static Dictionary<(int, int), GUIStyle> _blurredStylesByTextureSize = new Dictionary<(int, int), GUIStyle>();


        static void DrawCurtain(this Rect rect, Color color, int dir)
        {
            void genTextures()
            {
                if (_gradientTextures != null) return;

                _gradientTextures = new Texture2D[4];

                // var pixels = Enumerable.Range(0, 256).Select(r => Greyscale(1, r / 255f));
                var pixels = Enumerable.Range(0, 256).Select(r => Greyscale(1, (r / 255f).Smoothstep()));

                var up = new Texture2D(1, 256);
                up.SetPixels(pixels.Reverse().ToArray());
                up.Apply();
                up.hideFlags = HideFlags.DontSave;
                up.wrapMode = TextureWrapMode.Clamp;
                _gradientTextures[0] = up;

                var down = new Texture2D(1, 256);
                down.SetPixels(pixels.ToArray());
                down.Apply();
                down.hideFlags = HideFlags.DontSave;
                down.wrapMode = TextureWrapMode.Clamp;
                _gradientTextures[1] = down;

                var left = new Texture2D(256, 1);
                left.SetPixels(pixels.ToArray());
                left.Apply();
                left.hideFlags = HideFlags.DontSave;
                left.wrapMode = TextureWrapMode.Clamp;
                _gradientTextures[2] = left;

                var right = new Texture2D(256, 1);
                right.SetPixels(pixels.Reverse().ToArray());
                right.Apply();
                right.hideFlags = HideFlags.DontSave;
                right.wrapMode = TextureWrapMode.Clamp;
                _gradientTextures[3] = right;

            }
            void draw()
            {
                SetGUIColor(color);

                GUI.DrawTexture(rect, _gradientTextures[dir]);

                ResetGUIColor();

            }

            genTextures();
            draw();

        }
        public static void DrawCurtainUp(this Rect rect, Color color) => rect.DrawCurtain(color, 0);
        public static void DrawCurtainDown(this Rect rect, Color color) => rect.DrawCurtain(color, 1);
        public static void DrawCurtainLeft(this Rect rect, Color color) => rect.DrawCurtain(color, 2);
        public static void DrawCurtainRight(this Rect rect, Color color) => rect.DrawCurtain(color, 3);
        static Texture2D[] _gradientTextures;



        public static bool IsHovered(this Rect r) => !curEvent.isNull && r.Contains(curEvent.mousePosition);

        #endregion

        #region Spacing

        public static void Space(float px = 6) => GUILayout.Space(px);

        public static void Divider(float space = 15, float yOffset = 0)
        {
            GUILayout.Label("", GUILayout.Height(space), GUILayout.ExpandWidth(true));
            lastRect.SetHeightFromMid(1).SetWidthFromMid(lastRect.width - 16).MoveY(yOffset).Draw(isDarkTheme ? Color.white * .42f : Color.white * .72f);
        }

        public static Rect ExpandSpace() { GUILayout.Label("", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)); return lastRect; }

        public static Rect ExpandWidthLabelRect() { GUILayout.Label(""/* , GUILayout.Height(0) */, GUILayout.ExpandWidth(true)); return lastRect; }
        public static Rect ExpandWidthLabelRect(float height) { GUILayout.Label("", GUILayout.Height(height), GUILayout.ExpandWidth(true)); return lastRect; }


        #endregion

        #region Icons


        public static class EditorIcons
        {
            public static Texture2D GetIcon(string iconNameOrPath)
            {
                if (icons_byName.TryGetValue(iconNameOrPath, out var cachedResult)) return cachedResult;

                var icon = typeof(EditorGUIUtility).InvokeMethod<Texture2D>("LoadIcon", iconNameOrPath) as Texture2D;

                return icons_byName[iconNameOrPath] = icon;

            }

            static Dictionary<string, Texture2D> icons_byName = new Dictionary<string, Texture2D>();
        }




        #endregion

        #region Other 

        public static void MarkInteractive(this Rect rect)
        {
            if (!curEvent.isRepaint) return;

            var unclippedRect = (Rect)_mi_GUIClip_UnclipToWindow.Invoke(null, new object[] { rect });

            var curGuiView = _pi_GUIView_current.GetValue(null);

            _mi_GUIView_MarkHotRegion.Invoke(curGuiView, new object[] { unclippedRect });

        }
        static PropertyInfo _pi_GUIView_current = typeof(Editor).Assembly.GetType("UnityEditor.GUIView").GetProperty("current", maxBindingFlags);
        static MethodInfo _mi_GUIView_MarkHotRegion = typeof(Editor).Assembly.GetType("UnityEditor.GUIView").GetMethod("MarkHotRegion", maxBindingFlags);
        static MethodInfo _mi_GUIClip_UnclipToWindow = typeof(GUI).Assembly.GetType("UnityEngine.GUIClip").GetMethod("UnclipToWindow", maxBindingFlags, null, new[] { typeof(Rect) }, null);


        #endregion

    }

}
#endif