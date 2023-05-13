using System;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticLoadTypeColorSelector : IRefmaticLoadTypeSelector
    {
        public int Order => 1000;
        public Type ElementType => typeof(Color);
        public Type ObjectType => typeof(ColorObject);
        public object Convert(Type elementType, UnityEngine.Object obj)
        {
            return (obj as ColorObject)?.Color ?? default;
        }
    }
}
