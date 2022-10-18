using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Randoms.DailyReward.Editor
{
    using Internals;
    
    using Editor = UnityEditor.Editor;
    [CustomEditor (typeof (UnityEngine.Object), true)]
    [CanEditMultipleObjects]
    public class DailyRewardEditor : Editor
    {
        HelperBoxAttribute helperBox;
        List <(BtnAttribute, MethodInfo)> methods;
        
        private void OnEnable ()
        {
            helperBox = target.GetAttribute <HelperBoxAttribute> ();
            methods  = new List<(BtnAttribute, MethodInfo)> ();

            foreach (var method in target.GetMethodsWhereAttr <BtnAttribute> ())
            {
                methods.Add (method);
            } 
        }

        public override void OnInspectorGUI()
        {
            if (helperBox != null)
            {
                GUILayout.Space (5);
                GUILayout.Label (helperBox.helperText, EditorStyles.helpBox);   
                GUILayout.Space (5);
            }

            DrawDefaultInspector ();

            foreach (var (btnAttr,method) in methods)
            {
                if (method.GetParameters().Length == 0)
                {
                    if (GUILayout.Button (btnAttr.btnText, EditorStyles.miniButtonMid))
                    {
                        method.Invoke (target, null);
                    }
                }
            }
        }

    }
}


