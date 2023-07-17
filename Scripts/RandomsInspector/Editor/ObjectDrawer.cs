using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Randoms.Inspector
{
    using Internals.Reflection;

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Component), true)]
    public class ObjectDrawer : Editor
    {
        public static List<(MethodInfo method, System.Action action)> buttonMethods = new List<(MethodInfo, System.Action)>();

        void OnEnable()
        {
            buttonMethods.Clear();
            var methods = target.GetMethodsInfo((method) => {
                return method.IsDefined(typeof(ButtonAttribute), true);
            });

            foreach (var method in methods)
            {
                System.Action action = () => target.GetMethodInfo(method.Name).Invoke(target, null);
                buttonMethods.Add((method, action));
            }
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update ();

            var iterator = serializedObject.GetIterator();
            if (iterator.NextVisible(true))
            {
                do {
                    var fieldInfo = iterator.serializedObject.targetObject.GetType().GetField(iterator.propertyPath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (fieldInfo != null)
                    {
                        var showIfAttr = fieldInfo.GetCustomAttribute(typeof(ShowIfAttribute)) as ShowIfAttribute;
                        if(showIfAttr != null) {
                            var condition = serializedObject.targetObject
                                .GetMethodInfo(showIfAttr.Condition, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                .Invoke(serializedObject.targetObject, null) as bool?;
                            if(condition == null || !condition.Value) continue;
                        }
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(iterator, true);
                    }
                }
                while (iterator.NextVisible(false));
            }

            GUILayout.Space(10);
            foreach (var buttonMethod in buttonMethods)
            {
                var buttonAttr = buttonMethod.method.GetCustomAttribute(typeof(ButtonAttribute)) as ButtonAttribute;

                if (GUILayout.Button(buttonAttr.buttonName))
                {
                    buttonMethod.action.Invoke();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

    #endif
}

