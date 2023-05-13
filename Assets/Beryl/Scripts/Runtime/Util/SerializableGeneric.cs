using System;
using UnityEngine;

namespace Beryl.Util
{
    [Serializable]
    public sealed class SerializableGeneric<TKey, TValue>
    {
        [SerializeField] internal TKey key;
        [SerializeField] internal TValue value;
        public TValue Value => value;
        public SerializableGeneric() { }
        public SerializableGeneric(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public static class SerializableGenericExtensions
    {
        public static TValue Get<TKey, TValue>(this SerializableGeneric<TKey, TValue>[] self, TKey key)
        {
            foreach (var sg in self)
            {
                if (sg.key.Equals(key)) return sg.value;
            }
            return default;
        }
    }
}
