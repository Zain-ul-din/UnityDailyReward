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
        
        private void OnEnable ()
        {
            helperBox = target.GetAttribute <HelperBoxAttribute> ();
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
        }

    }
}


