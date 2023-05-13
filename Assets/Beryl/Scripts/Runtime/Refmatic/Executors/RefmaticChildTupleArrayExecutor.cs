using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticChildTupleArrayExecutor : IRefmaticExecutor
    {
        [SerializeField] int elmLen = default;
        [SerializeReference] IRefmaticElementChild[] elms = default;
        [SerializeReference] IRefmaticTupleSelector sl = default;
        [SerializeReference] IRefmaticGenericKeySelector gk = default;
        [SerializeReference] IRefmaticLoadTypeSelector[] lts = default;

        public void Execute(UnityEngine.Object obj, FieldInfo fi)
        {
            int maxLen = 0;
            RefmaticPools.Index.Clear();
            RefmaticPools.Transform.Clear();
            for (int i = 0; i < elmLen; i++)
            {
                var elem = elms[i];
                var prevLen = RefmaticPools.Transform.Length;
                RefmaticPools.Index.Add(prevLen);
                elem.CollectTransforms((obj as Component).transform, ref RefmaticPools.Transform);
                var nextLen = RefmaticPools.Transform.Length - prevLen;
                RefmaticPools.Index.Add(nextLen);
                if (nextLen > maxLen) maxLen = nextLen;
            }
            var map = new Transform[maxLen][];
            for (int i = 0; i < maxLen; i++)
                map[i] = new Transform[elmLen];
            for (int i = 0, len = RefmaticPools.Index.Length; i < len; i += 2)
            {
                int x = 0, y = i / 2;
                foreach (Transform t in new UnityObjectOrderBy<Transform>(
                    ref RefmaticPools.Transform, RefmaticPools.Index[i], RefmaticPools.Index[i + 1]))
                    map[x++][y] = t;
            }
            var type = fi.FieldType.GetElementType();
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

        public RefmaticChildTupleArrayExecutor(
            FieldInfo fi,
            IRefmaticElementChild[] elms,
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
