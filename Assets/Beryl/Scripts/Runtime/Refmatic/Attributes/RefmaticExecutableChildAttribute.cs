using System;
using System.Reflection;
using UnityEngine;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RefmaticExecutableChildAttribute : Attribute, IRefmaticExecutable
    {
        [SerializeReference] IRefmaticExecutor executor = default;

        public void Prepare(
            FieldInfo fi,
            IRefmaticElementChild[] childs,
            IRefmaticElementLoad[] loads,
            IRefmaticGenericKeySelector gk,
            IRefmaticLoadTypeSelector[] lts,
            IRefmaticSingleSelector sl1,
            IRefmaticTupleSelector sl2)
        {
            var elemLen = childs?.Length ?? 0;
            if (elemLen <= 0)
            {
                if (fi.FieldType.IsArray)
                    executor = new RefmaticChildSingleArrayExecutor(fi, new RefmaticElementChildAttribute(fi.Name), sl1, gk, lts);
                else
                    executor = new RefmaticChildSingleExecutor(fi, new RefmaticElementChildAttribute(fi.Name), sl1, gk, lts);
            }
            else if (elemLen == 1)
            {
                if (fi.FieldType.IsArray)
                    executor = new RefmaticChildSingleArrayExecutor(fi, childs[0], sl1, gk, lts);
                else
                    executor = new RefmaticChildSingleExecutor(fi, childs[0], sl1, gk, lts);
            }
            else
            {
                if (fi.FieldType.IsArray)
                    executor = new RefmaticChildTupleArrayExecutor(fi, childs, sl2, gk, lts);
                else
                    executor = new RefmaticChildTupleExecutor(fi, childs, sl2, gk, lts);
            }
        }

        public void Execute(UnityEngine.Object obj, FieldInfo fi)
        {
            executor.Execute(obj, fi);
        }

        public bool Referenceable(UnityEngine.Object[] objs)
        {
            return executor.Referenceable(objs);
        }
    }
}
