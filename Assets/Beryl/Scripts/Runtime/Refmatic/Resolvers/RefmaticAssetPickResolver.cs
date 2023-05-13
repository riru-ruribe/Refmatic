using System;
using UnityEngine;

namespace Beryl.Refmatic
{
    public sealed class RefmaticAssetPickResolver : RefmaticResolverBase<RefmaticAssetPickResolver, IRefmaticAssetPickSelector>
    {
        const string _Path = "Assets/Beryl/Assets/Editor/RefmaticAssetPickResolver.asset";

        protected override string Path => _Path;

        public IRefmaticAssetPickSelector Get(Type type)
        {
            var len = values.Length - 1;
            for (int i = 0; i < len; i++)
            {
                var ap = values[i];
                if (ap.Type == type) return ap;
            }
            return values[len];
        }

        [ContextMenu("Pick All")]
        void _PickAll()
        {
            PickAll();
        }
    }
}
