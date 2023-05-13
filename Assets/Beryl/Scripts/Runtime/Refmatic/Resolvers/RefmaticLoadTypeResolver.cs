using System;
using System.Linq;
using UnityEngine;

namespace Beryl.Refmatic
{
    public sealed class RefmaticLoadTypeResolver : RefmaticResolverBase<RefmaticLoadTypeResolver, IRefmaticLoadTypeSelector>
    {
        const string _Path = "Assets/Beryl/Assets/Editor/RefmaticLoadTypeResolver.asset";

        protected override string Path => _Path;

        public IRefmaticLoadTypeSelector Get(Type type)
        {
            var len = values.Length - 1;
            for (int i = 0; i < len; i++)
            {
                var lt = values[i];
                if (lt.ElementType == type) return lt;
            }
            return values[len];
        }

        public IRefmaticLoadTypeSelector[] Get(Type[] types)
        {
            return types.Select(x => Get(x)).ToArray();
        }

        [ContextMenu("Pick All")]
        void _PickAll()
        {
            PickAll();
        }
    }
}
