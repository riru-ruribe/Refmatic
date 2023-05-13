using System;

namespace Beryl.Refmatic
{
    sealed class RefmaticGenericKeyShortSelector : IRefmaticGenericKeySelector
    {
        public int Order => 3000;
        public Type Type => typeof(short);
        public object Convert(UnityEngine.Object obj)
        {
            return short.Parse(RefmaticParams.GenericKey.Replace(obj.name));
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
