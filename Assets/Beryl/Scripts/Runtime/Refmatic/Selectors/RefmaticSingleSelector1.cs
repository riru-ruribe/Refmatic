using System;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticSingleSelector1 : IRefmaticSingleSelector
    {
        public int Order => 1000;
        public Type MatchType(Type type, out Type keyType)
        {
            // SerializableGeneric < ? , ? >
            // SerializableGeneric < ? , ? > []
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(SerializableGeneric<,>))
                {
                    var args = type.GenericTypeArguments;
                    keyType = args[0];
                    return args[1];
                }
            }
            keyType = null;
            return null;
        }
        public object Select(Type type, IRefmaticGenericKeySelector gk, IRefmaticLoadTypeSelector lt, UnityEngine.Object obj)
        {
            var typeArgs = type.GenericTypeArguments;
            var length = typeArgs.Length;
            var args = new object[length];
            var i = length - 1;
            if (i > 0)
                args[0] = gk.Convert(obj);
            args[i] = lt.Convert(typeArgs[i], obj);
            return Activator.CreateInstance(type, args);
        }
    }
}
