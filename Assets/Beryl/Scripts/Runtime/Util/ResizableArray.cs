using System;

namespace Beryl.Util
{
    public struct ResizableArray<T>
    {
        T[] items;
        int len, capacity;

        public T this[int index] => items[index];

        public int Length
        {
            get => len;
            internal set => len = value;
        }

        public int Capacity => capacity;

        public T[] ToArray()
        {
            if (len == capacity) return items;
            var dst = new T[len];
            Array.Copy(items, dst, len);
            return dst;
        }

        public void Add(T item)
        {
            if (len >= capacity)
            {
                capacity += capacity;
                Array.Resize(ref items, capacity);
            }
            items[len++] = item;
        }

        public void Clear()
        {
            len = 0;
        }

        public ResizableArray(int capacity)
        {
            items = new T[capacity];
            len = 0;
            this.capacity = capacity;
        }
    }
}
