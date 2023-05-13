using System;
using UnityEngine;
using Beryl.Refmatic;
using Beryl.Util;

namespace Beryl.Samples
{
    // use case 'RefmaticGenericKeySelector' attribute.
    // when you want to convert the first generic argument to your own types.

    // by the way, default supports the following patterns.
    //   SerializableGeneric < int    , ? >
    //   SerializableGeneric < uint   , ? >
    //   SerializableGeneric < byte   , ? >
    //   SerializableGeneric < sbyte  , ? >
    //   SerializableGeneric < short  , ? >
    //   SerializableGeneric < ushort , ? >
    //   SerializableGeneric < long   , ? >
    //   SerializableGeneric < ulong  , ? >
    //   SerializableGeneric < bool   , ? >
    // key is determined by the numeric part of (? as UnityEngine.Object).name.
    sealed class RefmaticGenericKeyTest : MonoBehaviour, IAutoRefmaticImported
    {
#pragma warning disable 414
        const string Root = "Assets/Beryl/Samples/Runtime/Refmatic/";

        // implementation should be a class because reference using 'SerializeReference'.
        sealed class Selector : IRefmaticGenericKeySelector
        {
            public int Order => 0;
            public Type Type => null;
            public object Convert(UnityEngine.Object obj) // be called from 'IRefmaticSingleSelector'
            {
                switch (int.Parse(RefmaticParams.GenericKey.Replace(obj.name)))
                {
                    default:
                    case 1: return LogType.Log;
                    case 2: return LogType.Warning;
                    case 3: return LogType.Error;
                }
            }
            public object Convert(UnityEngine.Object[] objs) // be called from 'IRefmaticTupleSelector'
            {
                throw new NotImplementedException();
            }
        }

        [RefmaticGenericKeySelector(typeof(Selector))]
        [RefmaticElementLoad(@"log_img_\d{2,}", RefmaticComparisons.Regex, Root + "Loggers/")]
        [SerializeField, RefmaticExecutableLoad]
        SerializableGeneric<LogType, Sprite>[] sprites = default;

#pragma warning restore 414
    }
}
