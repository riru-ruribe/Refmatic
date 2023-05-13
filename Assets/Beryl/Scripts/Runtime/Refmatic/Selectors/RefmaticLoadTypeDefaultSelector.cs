using System;
using UnityEngine;

namespace Beryl.Refmatic
{
    sealed class RefmaticLoadTypeDefaultSelector : IRefmaticLoadTypeSelector
    {
        public int Order => int.MaxValue;
        public Type ElementType => null;
        public Type ObjectType => null;
        public object Convert(Type elementType, UnityEngine.Object obj)
        {
            if (obj is Transform t && obj.GetType() != elementType)
                return t.GetComponent(elementType);
            return obj;
        }
    }
}
