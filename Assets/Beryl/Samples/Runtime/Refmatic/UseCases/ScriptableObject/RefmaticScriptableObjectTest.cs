using UnityEngine;
using Beryl.Refmatic;
using Beryl.Util;

namespace Beryl.Samples
{
    // can also be used with 'ScriptableObject'.
    [CreateAssetMenu(menuName = "Beryl/Sample/RefmaticScriptableObjectTest", fileName = "RefmaticScriptableObjectTest")]
    sealed class RefmaticScriptableObjectTest : ScriptableObject
    {
#pragma warning disable 414
        const string Root = "Assets/Beryl/Samples/Runtime/Refmatic/";

        [RefmaticElementLoad(@"^beryl_clr_\d{2,}", RefmaticComparisons.Regex, Root + "Colors/")]
        [RefmaticElementLoad(@"^beryl_mat_\d{2,}", RefmaticComparisons.Regex, Root + "Materials/")]
        [SerializeField, RefmaticExecutableLoad]
        SerializableGeneric<int, SerializableTuple<Color, Material>>[] clrMatMap = default;

#pragma warning restore 414
    }
}
