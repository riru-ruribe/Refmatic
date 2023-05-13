using UnityEngine;
using UnityEngine.UI;

namespace Beryl.Samples
{
    abstract class TestLoggerBase : ITestLogger
    {
        [SerializeField] protected RectTransform rectTransform = default;
        [SerializeField] Sprite sprite = default;
        public virtual void Log(Transform parent)
        {
            if (sprite != null)
            {
                var go = new GameObject();
                go.name = sprite.name;
                go.transform.SetParent(parent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.AddComponent<Image>().sprite = sprite;
            }
        }
        public TestLoggerBase(RectTransform rectTransform)
        {
            this.rectTransform = rectTransform;
        }
        public TestLoggerBase(Sprite sprite)
        {
            this.sprite = sprite;
        }
    }
}
