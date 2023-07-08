using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Randoms.Internals.Reflection
{
    public static class ReflectionUtil
    {
        // Returns all FieldInfo objects that match the provided filter
        public static IEnumerable<FieldInfo> GetFieldsInfo(this UnityEngine.Object target, Func<FieldInfo, bool> filter)
        {
            Type targetTypeInfo = target.GetType();

            IEnumerable<FieldInfo> fields = targetTypeInfo
                .GetFields(ReflectionUtil.fieldsBindingFlags)
                .Where(filter);

            foreach (var field in fields)
            {
                yield return field;
            }
        }

        // Returns the FieldInfo object for a specific field name and binding flags
        public static FieldInfo GetFieldInfo(this UnityEngine.Object target, string fieldName, BindingFlags flags)
        {
            Type targetTypeInfo = target.GetType();
            return targetTypeInfo.GetField(fieldName, flags);
        }

        // Returns the value of a non-static field of type T
        public static T GetField<T>(this UnityEngine.Object target, string fieldName)
        {
            FieldInfo fieldInfo = ReflectionUtil.GetFieldInfo(target, fieldName, ReflectionUtil.NonStaticfieldBindingFlags);
            if (fieldInfo == null)
            {
                throw new NullReferenceException();
            }

            var fieldValue = fieldInfo.GetValue(target);
            if (fieldValue.GetType() != typeof(T))
            {
                throw new InvalidCastException();
            }

            return (T)fieldValue;
        }

        // Returns all MethodInfo objects that match the provided filter
        public static IEnumerable<MethodInfo> GetMethodsInfo(this UnityEngine.Object target, Func<MethodInfo, bool> filter)
        {
            Type targetTypeInfo = target.GetType();

            IEnumerable<MethodInfo> methods = targetTypeInfo
                .GetMethods(ReflectionUtil.methodsBindingFlags)
                .Where(filter);

            foreach (var method in methods)
            {
                yield return method;
            }
        }

        // Returns the MethodInfo object for a specific method name and binding flags
        public static MethodInfo GetMethodInfo(this UnityEngine.Object target, string methodName, BindingFlags flags)
        {
            Type targetTypeInfo = target.GetType();
            return targetTypeInfo.GetMethod(methodName, flags);
        }

        // Returns the MethodInfo object for a specific method name and binding flags
        public static MethodInfo GetMethodInfo(this UnityEngine.Object target, string methodName)
        {
            Type targetTypeInfo = target.GetType();
            return targetTypeInfo.GetMethod(methodName, methodsBindingFlags);
        }
        
        // Returns the value of a non-static field of type T
        public static T CallMethod<T>(this UnityEngine.Object target, string methodName, params object[] arguments)
        {
            MethodInfo methodInfo = ReflectionUtil.GetMethodInfo(target, methodName, ReflectionUtil.NonStaticmethodBindingFlags);
            if (methodInfo == null)
            {
                throw new NullReferenceException();
            }

            var result = methodInfo.Invoke(target, arguments);
            if (result.GetType() != typeof(T))
            {
                throw new InvalidCastException();
            }

            return (T)result;
        }

        // Binding flags for non-static fields
        public static BindingFlags NonStaticfieldBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;

        // Binding flags for all fields
        public static BindingFlags fieldsBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;

        // Binding flags for non-static methods
        public static BindingFlags NonStaticmethodBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
        
        // Binding flags for all methods
        public static BindingFlags methodsBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
    }
}
