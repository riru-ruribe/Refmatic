using System;
using System.Reflection;
using UnityEngine;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RefmaticExecutableMyselfAttribute : Attribute, IRefmaticExecutable
    {
        public void Prepare(
            FieldInfo fi,
            IRefmaticElementChild[] childs,
            IRefmaticElementLoad[] loads,
            IRefmaticGenericKeySelector gk,
            IRefmaticLoadTypeSelector[] lts,
            IRefmaticSingleSelector sl1,
            IRefmaticTupleSelector sl2)
        {
        }

        public void Execute(UnityEngine.Object obj, FieldInfo fi)
        {
            var type = fi.FieldType;
            if (type.IsArray)
            {
                type = type.GetElementType();
                var comps = (obj as Component).GetComponents(type);
                var dst = Array.CreateInstance(type, comps.Length);
                Array.Copy(comps, dst, comps.Length);
                fi.SetValue(obj, dst);
            }
            else
            {
                if (type == typeof(Transform))
                    fi.SetValue(obj, (obj as Component).transform);
                else if (type == typeof(GameObject))
                    fi.SetValue(obj, (obj as Component).gameObject);
                else
                    fi.SetValue(obj, (obj as Component).GetComponent(type));
            }
        }

        public bool Referenceable(UnityEngine.Object[] objs)
        {
            return false;
        }
    }
}
