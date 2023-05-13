using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Beryl.Refmatic
{
    sealed class RefmaticChildTupleExecutor : IRefmaticExecutor
    {
        [SerializeField] int elmLen = default;
        [SerializeReference] IRefmaticElementChild[] elms = default;
        [SerializeReference] IRefmaticTupleSelector sl = default;
        [SerializeReference] IRefmaticGenericKeySelector gk = default;
        [SerializeReference] IRefmaticLoadTypeSelector[] lts = default;

        public void Execute(UnityEngine.Object obj, FieldInfo fi)
        {
            RefmaticPools.Transform.Clear();
            for (int i = 0; i < elmLen; i++)
            {
                elms[i].CollectTransforms((obj as Component).transform, ref RefmaticPools.Transform);
                RefmaticPools.Transform.Length = i + 1; // truncate if multiple found.
            }
            fi.SetValue(obj, sl.Select(fi.FieldType, gk, lts, RefmaticPools.Transform.ToArray()));
        }

        public bool Referenceable(UnityEngine.Object[] objs)
        {
            for (int i = 0, len1 = objs.Length; i < len1; i++)
                for (int j = 0, len2 = elms.Length; j < len2; j++)
                    if (elms[j].IsMatch(objs[i].name)) return true;
            return false;
        }

        public RefmaticChildTupleExecutor(
            FieldInfo fi,
            IRefmaticElementChild[] elms,
            IRefmaticTupleSelector sl,
            IRefmaticGenericKeySelector gk,
            IRefmaticLoadTypeSelector[] lts)
        {
            var type = fi.FieldType;
            Type[] elementTypes = null;
            Type keyType = null;
            this.elms = elms;
            this.elmLen = elms.Length;
            this.sl = sl ?? RefmaticTupleResolver.Instance.Get(type, out elementTypes, out keyType);
            elementTypes ??= this.sl.MatchType(type, out keyType);
            this.gk = gk ?? RefmaticGenericKeyResolver.Instance.Get(keyType);

            var ltLen = lts?.Length ?? 0;
            if (ltLen == 0)
                this.lts = RefmaticLoadTypeResolver.Instance.Get(elementTypes);
            else if (ltLen == elementTypes.Length)
                this.lts = lts;
            else
            {
                this.lts = new IRefmaticLoadTypeSelector[elementTypes.Length];
                for (int i = 0, len = elementTypes.Length; i < len; i++)
                {
                    var t = elementTypes[i];
                    this.lts[i] = lts.FirstOrDefault(x => x.ElementType == t)
                        ?? RefmaticLoadTypeResolver.Instance.Get(t);
                }
            }
        }
    }
}
