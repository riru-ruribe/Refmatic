using UnityEngine;
using Beryl.Refmatic;
using Beryl.Util;

namespace Beryl.Samples
{
    sealed class RefmaticExecutableLoadTest : MonoBehaviour, IAutoRefmaticImported
    {
#pragma warning disable 414
        const string Root = "Assets/Beryl/Samples/Runtime/Refmatic/";

        // use case 'RefmaticExecutableLoad' attribute.
        // always give what to load with the 'RefmaticElementLoad' attribute.
        // recommended to specify searchInFolders in detail. because find for assets is very time consuming.
        #region RefmaticExecutableLoad

        [RefmaticElementLoad("Prefab", RefmaticComparisons.EndsWith, Root + "Prefabs/")]
        [SerializeField, RefmaticExecutableLoad]
        GameObject prefab = default;

        [RefmaticElementLoad(@"^beryl_clr_\d{2,}", RefmaticComparisons.Regex, Root + "Colors/")]
        [SerializeField, RefmaticExecutableLoad]
        Color[] clrs = default;

        #endregion

        // use case 'RefmaticElementLoad' attribute.
        // when you want to take multiple references.
        #region RefmaticElementLoad

        // field declaration is not an array so does not do 'OrderBy'.
        // so the first one found by 'AssetDatabase.FindAssets' is referenced.
        [RefmaticElementLoad("beryl_mat_", RefmaticComparisons.StartsWith, Root + "Materials/")]
        [RefmaticElementLoad("beryl_clr_", RefmaticComparisons.Regex, Root + "Colors/")]
        [SerializeField, RefmaticExecutableLoad]
        SerializableTuple<Material, Color> tuple1 = default;

        [RefmaticElementLoad("beryl_mat_", RefmaticComparisons.StartsWith, Root + "Materials/")]
        [RefmaticElementLoad(@"^beryl_clr_\d{2,}", RefmaticComparisons.Regex, Root + "Colors/")]
        [SerializeField, RefmaticExecutableLoad]
        SerializableGeneric<int, SerializableTuple<Material, Color>>[] tuple2 = default;

        #endregion
#pragma warning restore 414
    }
}
