using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Randoms.Inspector
{
    using Internals.Reflection;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(Component), true)]
    public class ObjectDrawer : Editor
    {
        public static List<(MethodInfo method, System.Action action)> buttonMethods = new List<(MethodInfo, System.Action)>();

        void OnEnable()
        {
            buttonMethods.Clear();
            var methods = target.GetMethodsInfo((method) =>
            {
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
            DrawDefaultInspector();
            GUILayout.Space(20);

            foreach (var buttonMethod in buttonMethods)
            {
                var buttonAttr = buttonMethod.method.GetCustomAttribute(typeof(ButtonAttribute)) as ButtonAttribute;

                if (GUILayout.Button(buttonAttr.buttonName))
                {
                    buttonMethod.action.Invoke();
                }
            }
        }
    }
}
