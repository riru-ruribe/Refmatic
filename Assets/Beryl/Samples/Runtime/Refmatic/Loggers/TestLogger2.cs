using UnityEngine;

namespace Beryl.Samples
{
    sealed class TestLogger2 : TestLoggerBase
    {
        public override void Log(Transform parent)
        {
            if (rectTransform != null)
            {
                Debug.LogWarning(rectTransform.name);
            }
            base.Log(parent);
        }
        public TestLogger2(RectTransform rectTransform) : base(rectTransform) { }
        public TestLogger2(Sprite sprite) : base(sprite) { }
    }
}
