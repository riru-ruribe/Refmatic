using System;

namespace Beryl.Refmatic
{
    /// <summary>
    /// change the serialization format of the loaded or found array objects.<br/>
    /// default supports the following patterns.<br/>
    /// <see cref="RefmaticTupleSelector1"/><br/>
    /// SerializableTuple { ?, ? }<br/>
    /// SerializableTuple { ?, ? } []<br/>
    /// <see cref="RefmaticTupleSelector2"/><br/>
    /// SerializableGeneric { ?, SerializableTuple { ?, ? } }<br/>
    /// SerializableGeneric { ?, SerializableTuple { ?, ? } }[]<br/>
    /// </summary>
    public interface IRefmaticTupleSelector : IRefmaticOrderable
    {
        Type[] MatchType(Type type, out Type keyType);
        object Select(Type type, IRefmaticGenericKeySelector gk, IRefmaticLoadTypeSelector[] lts, UnityEngine.Object[] objs);
    }
}
