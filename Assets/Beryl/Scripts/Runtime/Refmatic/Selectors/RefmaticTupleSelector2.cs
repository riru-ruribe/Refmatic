using System;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticTupleSelector2 : IRefmaticTupleSelector
    {
        public int Order => 2000;
        public Type[] MatchType(Type type, out Type keyType)
        {
            // SerializableGeneric < , SerializableTuple< ? , ? > >
            // SerializableGeneric < , SerializableTuple< ? , ? > > []
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(SerializableGeneric<,>))
                {
                    var args = type.GenericTypeArguments;
                    keyType = args[0];
                    type = args[1];
                    if (type.IsGenericType && RefmaticParams.TupleTypes.Contains(type.GetGenericTypeDefinition()))
                        return type.GenericTypeArguments;
                }
            }
            keyType = null;
            return null;
        }
        public object Select(Type type, IRefmaticGenericKeySelector gk, IRefmaticLoadTypeSelector[] lts, UnityEngine.Object[] objs)
        {
            var tupleTypeArgs = MatchType(type, out _);
            var tupleLength = tupleTypeArgs.Length;
            var args = new object[2];
            args[0] = gk.Convert(objs);
            var tupleArgs = new object[tupleLength];
            for (int i = 0; i < tupleLength; i++)
                tupleArgs[i] = lts[i].Convert(tupleTypeArgs[i], objs[i]);
            args[1] = Activator.CreateInstance(
                RefmaticParams.TupleTypes[tupleLength - 2].MakeGenericType(tupleTypeArgs),
                tupleArgs
            );
            return Activator.CreateInstance(type, args);
        }
    }
}
