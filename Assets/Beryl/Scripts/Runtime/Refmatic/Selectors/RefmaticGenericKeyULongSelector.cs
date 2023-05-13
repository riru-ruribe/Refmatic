using System;

namespace Beryl.Refmatic
{
    sealed class RefmaticGenericKeyULongSelector : IRefmaticGenericKeySelector
    {
        public int Order => 4001;
        public Type Type => typeof(ulong);
        public object Convert(UnityEngine.Object obj)
        {
            return ulong.Parse(RefmaticParams.GenericKey.Replace(obj.name));
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
