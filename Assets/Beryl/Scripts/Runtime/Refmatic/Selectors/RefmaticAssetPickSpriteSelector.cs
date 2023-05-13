using System;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticAssetPickSpriteSelector : IRefmaticAssetPickSelector
    {
        public int Order => 1000;
        public Type Type => typeof(Sprite);
        public void PickAsset(Type type, string path, IRefmaticElementLoad element, ref ResizableArray<UnityEngine.Object> array)
        {
#if UNITY_EDITOR
            // also supports multiple sprites. cast to Sprite to exclude Texture2D.
            foreach (var asset in UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path))
            {
                if (asset is Sprite sprite && element.IsMatch(asset.name))
                    array.Add(sprite);
            }
#endif
        }
    }
}
