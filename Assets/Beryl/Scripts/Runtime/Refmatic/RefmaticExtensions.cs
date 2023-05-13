using System;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    public static class RefmaticExtensions
    {
        internal static void CollectTransforms(this IRefmaticElementChild self, Transform parent, ref ResizableArray<Transform> array)
        {
            for (int i = 0, len = parent.childCount; i < len; i++)
            {
                var t = parent.GetChild(i);
                if (self.IsMatch(t.name)) array.Add(t);
                CollectTransforms(self, t, ref array);
            }
        }

        internal static void LoadAssets(this IRefmaticElementLoad self, Type type, ref ResizableArray<UnityEngine.Object> array)
        {
#if UNITY_EDITOR
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{type.Name}", self.SearchInFolders);
            if (guids == null || guids.Length <= 0) return;

            var picker = RefmaticAssetPickResolver.Instance.Get(type);
            foreach (var guid in guids)
                picker.PickAsset(type, UnityEditor.AssetDatabase.GUIDToAssetPath(guid), self, ref array);
#endif
        }
    }
}
