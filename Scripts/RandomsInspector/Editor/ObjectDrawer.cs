using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace Randoms.Inspector
{
    using Internals.Reflection;

    #if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Component), true)]
    public class ObjectDrawer : Editor
    {
        public static List<(MethodInfo method, System.Action action)> buttonMethods = new List<(MethodInfo, System.Action)>();
        static bool s_isUsingRandomsAttributes;

        void OnEnable()
        {
            buttonMethods.Clear();
            var methods = target.GetMethodsInfo((method) => {
                return method.IsDefined(typeof(ButtonAttribute), true);
            });

            var fields = target.GetFieldsInfo((field) => {
                return field.IsDefined(typeof(ShowIfAttribute), true);
            });

            
            foreach (var method in methods)
            {
                System.Action action = () => target.GetMethodInfo(method.Name).Invoke(target, null);
                buttonMethods.Add((method, action));
            }

            s_isUsingRandomsAttributes = methods.ToArray().Length > 0 || fields.ToArray().Length > 0;
        }
        
        public override void OnInspectorGUI()
        {
            if(!s_isUsingRandomsAttributes) {
                base.OnInspectorGUI();
                return;
            }

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

                if (buttonAttr.doc != "")
                    EditorGUILayout.HelpBox(buttonAttr.doc, MessageType.Info);
                
                if (GUILayout.Button(buttonAttr.buttonName))
                {
                    buttonMethod.action.Invoke();
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.Box("The Inspector is rendering via Randoms.Inspector");
            serializedObject.ApplyModifiedProperties();
        }
    }

    #endif
}

