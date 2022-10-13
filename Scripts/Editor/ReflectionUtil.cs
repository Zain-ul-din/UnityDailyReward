using System;
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

        public static BindingFlags NonStaticfieldBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
        public static BindingFlags fieldsBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
    }
}
