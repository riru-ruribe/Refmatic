using System;

namespace Beryl.Refmatic
{
    /// <summary>
    /// can change serialization format of TKey when using <see cref="SerializableGeneric{TKey, TValue}"/>.<br/>
    /// default supports the following patterns.<br/>
    ///  int    (order: 1000)<br/>
    ///  uint   (order: 1001)<br/>
    ///  byte   (order: 2000)<br/>
    ///  sbyte  (order: 2001)<br/>
    ///  short  (order: 3000)<br/>
    ///  ushort (order: 3001)<br/>
    ///  long   (order: 4000)<br/>
    ///  ulong  (order: 4001)<br/>
    ///  bool   (order: 5000) 
    /// </summary>
    public interface IRefmaticGenericKeySelector : IRefmaticOrderable
    {
        Type Type { get; }
        object Convert(UnityEngine.Object obj);
        object Convert(UnityEngine.Object[] objs);
    }
}
