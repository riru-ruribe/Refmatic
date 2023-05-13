using System;

namespace Beryl.Refmatic
{
    /// <summary>
    /// change the serialization format of the loaded or found object.<br/>
    /// default supports the following patterns.<br/>
    /// <see cref="RefmaticSingleSelector1"/><br/>
    /// SerializableGeneric { ?, ? }<br/>
    /// SerializableGeneric { ?, ? } []<br/>
    /// <see cref="RefmaticSingleSelector2"/><br/>
    /// all other classes.
    /// </summary>
    public interface IRefmaticSingleSelector : IRefmaticOrderable
    {
        Type MatchType(Type type, out Type keyType);
        object Select(Type type, IRefmaticGenericKeySelector gk, IRefmaticLoadTypeSelector lt, UnityEngine.Object obj);
    }
}
