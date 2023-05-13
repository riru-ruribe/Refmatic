using System;

namespace Beryl.Refmatic
{
    sealed class RefmaticGenericKeyUShortSelector : IRefmaticGenericKeySelector
    {
        public int Order => 3001;
        public Type Type => typeof(ushort);
        public object Convert(UnityEngine.Object obj)
        {
            return ushort.Parse(RefmaticParams.GenericKey.Replace(obj.name));
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
