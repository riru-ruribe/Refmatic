using System;
using UnityEngine;

namespace Beryl.Refmatic
{
    public sealed class RefmaticGenericKeyResolver : RefmaticResolverBase<RefmaticGenericKeyResolver, IRefmaticGenericKeySelector>
    {
        const string _Path = "Assets/Beryl/Assets/Editor/RefmaticGenericKeyResolver.asset";

        protected override string Path => _Path;

        public IRefmaticGenericKeySelector Get(Type type)
        {
            for (int i = 0, len = values.Length; i < len; i++)
            {
                var gk = values[i];
                if (gk.Type == type) return gk;
            }
            return null;
        }

        [ContextMenu("Pick All")]
        void _PickAll()
        {
            PickAll();
        }
    }
}
