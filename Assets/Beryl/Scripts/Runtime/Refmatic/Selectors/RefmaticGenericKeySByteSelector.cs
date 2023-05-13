using System;

namespace Beryl.Refmatic
{
    sealed class RefmaticGenericKeySByteSelector : IRefmaticGenericKeySelector
    {
        public int Order => 2001;
        public Type Type => typeof(sbyte);
        public object Convert(UnityEngine.Object obj)
        {
            return sbyte.Parse(RefmaticParams.GenericKey.Replace(obj.name));
        }
        public object Convert(UnityEngine.Object[] objs)
        {
            UnityEngine.Object obj = null;
            for (int i = 0, len = objs.Length; i < len; i++)
            {
                obj = objs[i];
                if (obj != null) break;
            }
            return Convert(obj);
        }
    }
}
