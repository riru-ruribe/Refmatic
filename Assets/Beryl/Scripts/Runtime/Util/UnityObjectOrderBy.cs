using System;
using System.Collections;
using System.Collections.Generic;

namespace Beryl.Util
{
    public struct UnityObjectOrderBy<T> : IEnumerable where T : UnityEngine.Object
    {
        sealed class UnityObjectOrderByEnumerator : IEnumerator
        {
            public int idx, poolSize;
            public object Current => pool[idx];
            public bool MoveNext() => ++idx < poolSize;
            public void Reset() { }
        }

        static List<T> pool = new List<T>(256);
        static UnityObjectOrderByEnumerator enumerator = new UnityObjectOrderByEnumerator();

        public IEnumerator GetEnumerator()
        {
            return enumerator;
        }

        public UnityObjectOrderBy(ref ResizableArray<T> ary)
        {
            pool.Clear();
            int poolSize = 0;
            var len = ary.Length;
            for (int i = 0; i < len; i++)
            {
                var obj = ary[i];
                for (int j = 0; j < len; j++)
                {
                    if (j >= poolSize)
                    {
                        pool.Add(obj);
                        poolSize++;
                        break;
                    }
                    if (StringComparer.Ordinal.Compare(obj.name, pool[j].name) < 0)
                    {
                        pool.Insert(j, obj);
                        poolSize++;
                        break;
                    }
                }
            }
            enumerator.idx = -1;
            enumerator.poolSize = poolSize;
        }

        public UnityObjectOrderBy(ref ResizableArray<T> ary, int start, int length)
        {
            pool.Clear();
            int poolSize = 0;
            for (int i = 0; i < length; i++)
            {
                var obj = ary[i + start];
                for (int j = 0; j < length; j++)
                {
                    if (j >= poolSize)
                    {
                        pool.Add(obj);
                        poolSize++;
                        break;
                    }
                    if (StringComparer.Ordinal.Compare(obj.name, pool[j].name) < 0)
                    {
                        pool.Insert(j, obj);
                        poolSize++;
                        break;
                    }
                }
            }
            enumerator.idx = -1;
            enumerator.poolSize = poolSize;
        }
    }
}
