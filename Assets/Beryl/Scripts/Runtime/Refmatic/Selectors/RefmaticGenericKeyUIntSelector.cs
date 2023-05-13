using System;

namespace Beryl.Refmatic
{
    sealed class RefmaticGenericKeyUIntSelector : IRefmaticGenericKeySelector
    {
        public int Order => 1001;
        public Type Type => typeof(uint);
        public object Convert(UnityEngine.Object obj)
        {
            return uint.Parse(RefmaticParams.GenericKey.Replace(obj.name));
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
