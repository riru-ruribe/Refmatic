using UnityEngine;
using UnityEngine.UI;
using Beryl.Refmatic;
using Beryl.Util;

namespace Beryl.Samples
{
    sealed class RefmaticExecutableChildTest : MonoBehaviour, IAutoRefmatic
    {
#pragma warning disable 414
        // use case 'RefmaticExecutableChild' attribute.
        #region RefmaticExecutableChild

        [SerializeField, RefmaticExecutableChild]
        Transform t = default;

        [RefmaticElementChild("aaaaa")]
        [SerializeField, RefmaticExecutableChild]
        Transform t2 = default;

        [RefmaticElementChild("ary_", RefmaticComparisons.StartsWith)]
        [SerializeField, RefmaticExecutableChild]
        Transform[] ary = default;

        #endregion

        // use case 'RefmaticElementChild' attribute.
        // when you want to take multiple references.
        #region RefmaticElementChild

        // field declaration is not an array so does not do 'OrderBy'.
        // so the first one found by 'GetComponentsInChildren' is referenced.
        [RefmaticElementChild("ary_", RefmaticComparisons.StartsWith)]
        [RefmaticElementChild("img_", RefmaticComparisons.StartsWith)]
        [SerializeField, RefmaticExecutableChild]
        SerializableTuple<Transform, Image> tuple1 = default;

        [RefmaticElementChild("ary_", RefmaticComparisons.StartsWith)]
        [RefmaticElementChild("img_", RefmaticComparisons.StartsWith)]
        [SerializeField, RefmaticExecutableChild]
        SerializableGeneric<int, SerializableTuple<Transform, Image>>[] tuple2 = default;

        #endregion
#pragma warning restore 414
    }
}
