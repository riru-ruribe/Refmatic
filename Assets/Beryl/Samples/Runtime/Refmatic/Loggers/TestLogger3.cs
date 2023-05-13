using UnityEngine;

namespace Beryl.Samples
{
    sealed class TestLogger3 : TestLoggerBase
    {
        public override void Log(Transform parent)
        {
            if (rectTransform != null)
            {
                Debug.LogError(rectTransform.name);
            }
            base.Log(parent);
        }
        public TestLogger3(RectTransform rectTransform) : base(rectTransform) { }
        public TestLogger3(Sprite sprite) : base(sprite) { }
    }
}
