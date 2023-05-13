using System;
using UnityEngine;

namespace Beryl.Refmatic
{
    public sealed class RefmaticSingleResolver : RefmaticResolverBase<RefmaticSingleResolver, IRefmaticSingleSelector>
    {
        const string _Path = "Assets/Beryl/Assets/Editor/RefmaticSingleResolver.asset";

        protected override string Path => _Path;

        public IRefmaticSingleSelector Get(Type type, out Type elementType, out Type keyType)
        {
            for (int i = 0, len = values.Length; i < len; i++)
            {
                var sl = values[i];
                elementType = sl.MatchType(type, out keyType);
                if (elementType != null) return sl;
            }
            elementType = null;
            keyType = null;
            return null;
        }

        [ContextMenu("Pick All")]
        void _PickAll()
        {
            PickAll();
        }
    }
}
