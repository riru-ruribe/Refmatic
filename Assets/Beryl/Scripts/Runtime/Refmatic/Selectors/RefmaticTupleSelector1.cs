using System;

namespace Beryl.Refmatic
{
    sealed class RefmaticTupleSelector1 : IRefmaticTupleSelector
    {
        public int Order => 1000;
        public Type[] MatchType(Type type, out Type keyType)
        {
            // SerializableTuple< ? , ? >
            // SerializableTuple< ? , ? > []
            keyType = null;
            if (type.IsGenericType)
            {
                var definition = type.GetGenericTypeDefinition();
                if (RefmaticParams.TupleTypes.Contains(definition))
                    return type.GenericTypeArguments;
            }
            return null;
        }
        public object Select(Type type, IRefmaticGenericKeySelector gk, IRefmaticLoadTypeSelector[] lts, UnityEngine.Object[] objs)
        {
            var tupleTypeArgs = type.GenericTypeArguments;
            var tupleLength = tupleTypeArgs.Length;
            var args = new object[tupleLength];
            for (int i = 0; i < tupleLength; i++)
                args[i] = lts[i].Convert(tupleTypeArgs[i], objs[i]);
            return Activator.CreateInstance(type, args);
        }
    }
}
