using UnityEngine;
using Beryl.Util;

namespace Beryl.Extensions
{
    public static class TransformExtensions
    {
        public static string NameWithParent(this Transform self)
        {
#if UNITY_2021_2_OR_NEWER
            var sb = new UniSpanSb(2048);
            sb.NameWithParent(self);
            return sb.ResultWithDispose;
#else
            var ret = self?.name;
            var t = self?.parent;
            while (t != null)
            {
                ret = $"{t.name}/{ret}";
                t = t.parent;
            }
            return ret;
#endif
        }
    }
}
