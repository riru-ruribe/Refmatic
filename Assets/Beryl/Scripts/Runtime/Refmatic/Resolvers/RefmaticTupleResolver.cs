using System;
using UnityEngine;

namespace Beryl.Refmatic
{
    public sealed class RefmaticTupleResolver : RefmaticResolverBase<RefmaticTupleResolver, IRefmaticTupleSelector>
    {
        const string _Path = "Assets/Beryl/Assets/Editor/RefmaticTupleResolver.asset";

        protected override string Path => _Path;

        public IRefmaticTupleSelector Get(Type type, out Type[] elementTypes, out Type keyType)
        {
            for (int i = 0, len = values.Length; i < len; i++)
            {
                var sl = values[i];
                elementTypes = sl.MatchType(type, out keyType);
                if (elementTypes != null) return sl;
            }
            elementTypes = null;
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
