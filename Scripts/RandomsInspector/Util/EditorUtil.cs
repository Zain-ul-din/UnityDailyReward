using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Randoms.Inspector
{
    using DailyReward;
    public static class EditorUtil 
    {
        public static void FocusOrCreateAsset (
            string assetFilter,
            bool allowMultiple = false
        )
        {
            var potentialsAssets = UnityEditor.AssetDatabase.FindAssets(assetFilter);

            if (potentialsAssets.Length == 0)
            {
                var asset = ScriptableObject.CreateInstance<DailyRewardConfigSO>();
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    AssetDatabase.CreateFolder("Assets", "Resources");
                AssetDatabase.CreateAsset(asset, assetFilter);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Selection.activeObject = asset;
                return;
            }
            else if (potentialsAssets.Length > 1 && !allowMultiple) {
                Debug.LogError("Multiple DailyRewardConfigSO found in project. Please delete all and create new one");
                return;
            }

            foreach(var asset in  potentialsAssets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                if (path.Contains(assetFilter))
                {
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<DailyRewardConfigSO>(path);
                    return;
                }
            }
        } 
    }
}
