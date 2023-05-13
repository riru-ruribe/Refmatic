using System;
using UnityEngine;

namespace Beryl.Util
{
    [Serializable]
    public sealed class SerializableTuple<T1, T2>
    {
        [SerializeField] internal T1 item1;
        [SerializeField] internal T2 item2;
        public T1 Item1 => item1;
        public T2 Item2 => item2;
        public SerializableTuple() { }
        public SerializableTuple(T1 item1, T2 item2)
        {
            this.item1 = item1;
            this.item2 = item2;
        }
    }

    [Serializable]
    public sealed class SerializableTuple<T1, T2, T3>
    {
        [SerializeField] internal T1 item1;
        [SerializeField] internal T2 item2;
        [SerializeField] internal T3 item3;
        public T1 Item1 => item1;
        public T2 Item2 => item2;
        public T3 Item3 => item3;
        public SerializableTuple() { }
        public SerializableTuple(T1 item1, T2 item2, T3 item3)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
        }
    }

    [Serializable]
    public sealed class SerializableTuple<T1, T2, T3, T4>
    {
        [SerializeField] internal T1 item1;
        [SerializeField] internal T2 item2;
        [SerializeField] internal T3 item3;
        [SerializeField] internal T4 item4;
        public T1 Item1 => item1;
        public T2 Item2 => item2;
        public T3 Item3 => item3;
        public T4 Item4 => item4;
        public SerializableTuple() { }
        public SerializableTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
        }
    }
}
