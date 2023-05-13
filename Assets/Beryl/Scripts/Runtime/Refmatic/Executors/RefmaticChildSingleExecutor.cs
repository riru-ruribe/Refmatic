using System;
using System.Reflection;
using UnityEngine;

namespace Beryl.Refmatic
{
    sealed class RefmaticChildSingleExecutor : IRefmaticExecutor
    {
        [SerializeReference] IRefmaticElementChild elm = default;
        [SerializeReference] IRefmaticSingleSelector sl = default;
        [SerializeReference] IRefmaticGenericKeySelector gk = default;
        [SerializeReference] IRefmaticLoadTypeSelector lt = default;

        public void Execute(UnityEngine.Object obj, FieldInfo fi)
        {
            RefmaticPools.Transform.Clear();
            elm.CollectTransforms((obj as Component).transform, ref RefmaticPools.Transform);
            fi.SetValue(obj, RefmaticPools.Transform.Length > 0
                ? sl.Select(fi.FieldType, gk, lt, RefmaticPools.Transform[0])
                : null);
        }

        public bool Referenceable(UnityEngine.Object[] objs)
        {
            for (int i = 0, len = objs.Length; i < len; i++)
                if (elm.IsMatch(objs[i].name)) return true;
            return false;
        }

        public RefmaticChildSingleExecutor(
            FieldInfo fi,
            IRefmaticElementChild elm,
            IRefmaticSingleSelector sl,
            IRefmaticGenericKeySelector gk,
            IRefmaticLoadTypeSelector[] lts)
        {
            var type = fi.FieldType;
            Type elementType = null;
            Type keyType = null;
            this.elm = elm;
            this.sl = sl ?? RefmaticSingleResolver.Instance.Get(type, out elementType, out keyType);
            this.gk = gk ?? RefmaticGenericKeyResolver.Instance.Get(keyType);
            this.lt = lts?.Length > 0 ? lts[0] : RefmaticLoadTypeResolver.Instance.Get(elementType);
        }
    }
}
