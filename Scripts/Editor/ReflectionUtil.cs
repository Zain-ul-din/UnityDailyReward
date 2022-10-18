using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Randoms.DailyReward.Editor.Internals
{
    internal static class ReflectionUtil
    {

        public static Attr GetAttribute <Attr> (this UnityEngine.Object target, bool inherit = false) 
            where Attr: Attribute
        {
            var target_T = target.GetType ();
            if (!target_T.IsDefined (typeof(Attr), inherit))
            {
                return null;
            }
            return target_T.GetCustomAttribute (typeof (Attr)) as Attr;
        }    


        public static IEnumerable <MethodInfo> GetMethods (this UnityEngine.Object target, bool inherit = false)
        {
            var target_T = target.GetType ();
            foreach (var method  in target_T.GetMethods (methodBindingFlags))
            {
                yield return method;
            }
        } 


        public static IEnumerable <(Attr, MethodInfo)> GetMethodsWhereAttr <Attr> (this UnityEngine.Object target, bool inherit =false)
            where Attr : Attribute
        {
            foreach (var method in GetMethods (target, inherit))
            {
                if (method.IsDefined (typeof (Attr), inherit))
                {
                    var attr = method.GetCustomAttribute(typeof(Attr),inherit) as Attr;
                    yield return (attr, method);
                }
            }
        }

        public static BindingFlags methodBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.InvokeMethod;
        public static BindingFlags nonStaticfieldBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
        public static BindingFlags fieldsBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
    }
}
