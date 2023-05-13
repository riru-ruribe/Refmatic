using UnityEngine;
using UnityEngine.UI;
using Beryl.Refmatic;

namespace Beryl.Samples
{
    sealed class RefmaticImageCounterTest : MonoBehaviour
    {
        const string Root = "Assets/Beryl/Samples/Runtime/Refmatic/";

        [RefmaticElementChild("img_", RefmaticComparisons.StartsWith)]
        [SerializeField, RefmaticExecutableChild]
        Image[] images = default;

        [RefmaticElementLoad("beryl_num_", RefmaticComparisons.StartsWith, Root + "NumSprites/")]
        [SerializeField, RefmaticExecutableLoad]
        Sprite[] sprites = default;

        [ContextMenu("SetRandom")]
        void SetRandom()
        {
            var r = UnityEngine.Random.Range(0, 10 * images.Length);
            Debug.Log(r);

            for (int i = 0; i < images.Length; i++)
            {
                if (r <= 0)
                {
                    var enabled = i == 0;
                    images[i].enabled = enabled;
                    images[i].sprite = enabled ? sprites[0] : null;
                    continue;
                }

                images[i].enabled = true;
                images[i].sprite = sprites[r % 10];

                r = (int)(r * 0.1);
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
