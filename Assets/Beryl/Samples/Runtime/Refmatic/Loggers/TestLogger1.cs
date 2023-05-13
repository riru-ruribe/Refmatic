using UnityEngine;

namespace Beryl.Samples
{
    sealed class TestLogger1 : TestLoggerBase
    {
        public override void Log(Transform parent)
        {
            if (rectTransform != null)
            {
                Debug.Log(rectTransform.name);
            }
            base.Log(parent);
        }
        public TestLogger1(RectTransform rectTransform) : base(rectTransform) { }
        public TestLogger1(Sprite sprite) : base(sprite) { }
    }
}
