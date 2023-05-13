using System;

namespace Beryl.Refmatic
{
    sealed class RefmaticSingleSelector2 : IRefmaticSingleSelector
    {
        public int Order => int.MaxValue;
        public Type MatchType(Type type, out Type keyType)
        {
            keyType = null;
            return type;
        }
        public object Select(Type type, IRefmaticGenericKeySelector gk, IRefmaticLoadTypeSelector lt, UnityEngine.Object obj)
        {
            return lt.Convert(type, obj);
        }
    }
}
