using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    public static class RefmaticPools
    {
        public static ResizableArray<UnityEngine.Object> Object = new ResizableArray<UnityEngine.Object>(256);
        public static ResizableArray<Transform> Transform = new ResizableArray<Transform>(256);
        public static ResizableArray<int> Index = new ResizableArray<int>(32);
    }
}
