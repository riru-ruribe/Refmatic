using System;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticLoadTypeMonoScriptSelector : IRefmaticLoadTypeSelector
    {
        public int Order => 2000;
        public Type ElementType => typeof(BerylMonoScript);
#if UNITY_EDITOR
        public Type ObjectType => typeof(UnityEditor.MonoScript);
#else
        public Type ObjectType => null;
#endif
        public object Convert(Type elementType, UnityEngine.Object obj)
        {
            return obj;
        }
    }
}
