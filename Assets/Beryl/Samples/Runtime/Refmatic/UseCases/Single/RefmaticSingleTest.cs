using System;
using UnityEngine;
using UnityEngine.UI;
using Beryl.Refmatic;

namespace Beryl.Samples
{
    // use case 'RefmaticSingleSelector' attribute.
    // when the referenced field class itself is of its own types.
    sealed class RefmaticSingleTest : MonoBehaviour, IAutoRefmatic
    {
#pragma warning disable 414

        [Serializable]
        sealed class RtAndImg
        {
            public RectTransform Rt;
            public Image Img;
        }

        // implementation should be a class because reference using 'SerializeReference'.
        sealed class Selector : IRefmaticSingleSelector
        {
            public int Order => 0;
            public Type MatchType(Type type, out Type keyType)
            {
                keyType = null;
                return typeof(RectTransform);
            }
            public object Select(Type type, IRefmaticGenericKeySelector gk, IRefmaticLoadTypeSelector lt, UnityEngine.Object obj)
            {
                var rt = obj as RectTransform;
                return new RtAndImg
                {
                    Rt = rt,
                    Img = rt?.GetComponent<Image>(),
                };
            }
        }

        [RefmaticSingleSelector(typeof(Selector))]
        [RefmaticElementChild("img_", RefmaticComparisons.StartsWith)]
        [SerializeField, RefmaticExecutableChild]
        RtAndImg[] rts = default;

#pragma warning restore 414
    }
}
