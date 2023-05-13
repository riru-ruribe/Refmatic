using System;
using System.Reflection;
using UnityEngine;

namespace Beryl.Refmatic
{
    sealed class RefmaticLoadSingleExecutor : IRefmaticExecutor
    {
        [SerializeReference] IRefmaticElementLoad elm = default;
        [SerializeReference] IRefmaticSingleSelector sl = default;
        [SerializeReference] IRefmaticGenericKeySelector gk = default;
        [SerializeReference] IRefmaticLoadTypeSelector lt = default;

        public void Execute(UnityEngine.Object obj, FieldInfo fi)
        {
            var type = fi.FieldType;
            RefmaticPools.Object.Clear();
            elm.LoadAssets(lt.ObjectType ?? sl.MatchType(type, out _), ref RefmaticPools.Object);
            fi.SetValue(obj, RefmaticPools.Object.Length > 0
                ? sl.Select(type, gk, lt, RefmaticPools.Object[0])
                : null);
        }

        public bool Referenceable(UnityEngine.Object[] objs)
        {
            for (int i = 0, len = objs.Length; i < len; i++)
                if (elm.IsMatch(objs[i].name)) return true;
            return false;
        }

        public RefmaticLoadSingleExecutor(
            FieldInfo fi,
            IRefmaticElementLoad elm,
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
