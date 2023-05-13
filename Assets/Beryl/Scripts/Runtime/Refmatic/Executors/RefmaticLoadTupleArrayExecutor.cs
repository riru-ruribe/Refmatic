using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticLoadTupleArrayExecutor : IRefmaticExecutor
    {
        [SerializeField] int elmLen = default;
        [SerializeReference] IRefmaticElementLoad[] elms = default;
        [SerializeReference] IRefmaticTupleSelector sl = default;
        [SerializeReference] IRefmaticGenericKeySelector gk = default;
        [SerializeReference] IRefmaticLoadTypeSelector[] lts = default;

        public void Execute(UnityEngine.Object obj, FieldInfo fi)
        {
            var type = fi.FieldType.GetElementType();
            var elementTypes = sl.MatchType(type, out _);
            int maxLen = 0;
            RefmaticPools.Index.Clear();
            RefmaticPools.Object.Clear();
            for (int i = 0; i < elmLen; i++)
            {
                var prevLen = RefmaticPools.Object.Length;
                RefmaticPools.Index.Add(prevLen);
                elms[i].LoadAssets(lts[i].ObjectType ?? elementTypes[i], ref RefmaticPools.Object);
                var nextLen = RefmaticPools.Object.Length - prevLen;
                RefmaticPools.Index.Add(nextLen);
                if (nextLen > maxLen) maxLen = nextLen;
            }
            var map = new UnityEngine.Object[maxLen][];
            for (int i = 0; i < maxLen; i++)
                map[i] = new UnityEngine.Object[elmLen];
            for (int i = 0, len = RefmaticPools.Index.Length; i < len; i += 2)
            {
                int x = 0, y = i / 2;
                foreach (UnityEngine.Object o in new UnityObjectOrderBy<UnityEngine.Object>(
                    ref RefmaticPools.Object, RefmaticPools.Index[i], RefmaticPools.Index[i + 1]))
                    map[x++][y] = o;
            }
            var dst = Array.CreateInstance(type, maxLen);
            var src = new object[maxLen];
            for (int i = 0; i < maxLen; i++)
                src[i] = sl.Select(type, gk, lts, map[i]);
            Array.Copy(src, dst, maxLen);
            fi.SetValue(obj, dst);
        }

        public bool Referenceable(UnityEngine.Object[] objs)
        {
            for (int i = 0, len1 = objs.Length; i < len1; i++)
                for (int j = 0, len2 = elms.Length; j < len2; j++)
                    if (elms[j].IsMatch(objs[i].name)) return true;
            return false;
        }

        public RefmaticLoadTupleArrayExecutor(
            FieldInfo fi,
            IRefmaticElementLoad[] elms,
            IRefmaticTupleSelector sl,
            IRefmaticGenericKeySelector gk,
            IRefmaticLoadTypeSelector[] lts)
        {
            var type = fi.FieldType.GetElementType();
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
