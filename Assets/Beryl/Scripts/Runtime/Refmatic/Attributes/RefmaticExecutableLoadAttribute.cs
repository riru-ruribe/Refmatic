using System;
using System.Reflection;
using UnityEngine;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RefmaticExecutableLoadAttribute : Attribute, IRefmaticExecutable
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
            var elemLen = loads?.Length ?? 0;
            if (elemLen <= 0)
            {
                if (fi.FieldType.IsArray)
                    executor = new RefmaticLoadSingleArrayExecutor(fi, new RefmaticElementLoadAttribute(fi.Name), sl1, gk, lts);
                else
                    executor = new RefmaticLoadSingleExecutor(fi, new RefmaticElementLoadAttribute(fi.Name), sl1, gk, lts);
            }
            else if (elemLen == 1)
            {
                if (fi.FieldType.IsArray)
                    executor = new RefmaticLoadSingleArrayExecutor(fi, loads[0], sl1, gk, lts);
                else
                    executor = new RefmaticLoadSingleExecutor(fi, loads[0], sl1, gk, lts);
            }
            else
            {
                if (fi.FieldType.IsArray)
                    executor = new RefmaticLoadTupleArrayExecutor(fi, loads, sl2, gk, lts);
                else
                    executor = new RefmaticLoadTupleExecutor(fi, loads, sl2, gk, lts);
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
