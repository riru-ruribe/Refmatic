using System;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticAssetPickDefaultSelector : IRefmaticAssetPickSelector
    {
        public int Order => int.MaxValue;
        public Type Type => null;
        public void PickAsset(Type type, string path, IRefmaticElementLoad element, ref ResizableArray<UnityEngine.Object> array)
        {
#if UNITY_EDITOR
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath(path, type);
            if (element.IsMatch(asset.name))
                array.Add(asset);
#endif
        }
    }
}
