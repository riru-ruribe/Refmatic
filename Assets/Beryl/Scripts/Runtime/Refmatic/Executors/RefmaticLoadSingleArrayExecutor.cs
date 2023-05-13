using System;
using System.Reflection;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticLoadSingleArrayExecutor : IRefmaticExecutor
    {
        [SerializeReference] IRefmaticElementLoad elm = default;
        [SerializeReference] IRefmaticSingleSelector sl = default;
        [SerializeReference] IRefmaticGenericKeySelector gk = default;
        [SerializeReference] IRefmaticLoadTypeSelector lt = default;

        public void Execute(UnityEngine.Object obj, FieldInfo fi)
        {
            var type = fi.FieldType.GetElementType();
            RefmaticPools.Object.Clear();
            elm.LoadAssets(lt.ObjectType ?? sl.MatchType(type, out _), ref RefmaticPools.Object);
            var len = RefmaticPools.Object.Length;
            if (len <= 0)
            {
                fi.SetValue(obj, null);
                return;
            }
            var dst = Array.CreateInstance(type, len);
            var src = new object[len];
            int i = 0;
            foreach (UnityEngine.Object o in new UnityObjectOrderBy<UnityEngine.Object>(ref RefmaticPools.Object))
                src[i++] = sl.Select(type, gk, lt, o);
            Array.Copy(src, dst, len);
            fi.SetValue(obj, dst);
        }

        public bool Referenceable(UnityEngine.Object[] objs)
        {
            for (int i = 0, len = objs.Length; i < len; i++)
                if (elm.IsMatch(objs[i].name)) return true;
            return false;
        }

        public RefmaticLoadSingleArrayExecutor(
            FieldInfo fi,
            IRefmaticElementLoad elm,
            IRefmaticSingleSelector sl,
            IRefmaticGenericKeySelector gk,
            IRefmaticLoadTypeSelector[] lts)
        {
            var type = fi.FieldType.GetElementType();
            Type elementType = null;
            Type keyType = null;
            this.elm = elm;
            this.sl = sl ?? RefmaticSingleResolver.Instance.Get(type, out elementType, out keyType);
            this.gk = gk ?? RefmaticGenericKeyResolver.Instance.Get(keyType);
            this.lt = lts?.Length > 0 ? lts[0] : RefmaticLoadTypeResolver.Instance.Get(elementType);
        }
    }
}
