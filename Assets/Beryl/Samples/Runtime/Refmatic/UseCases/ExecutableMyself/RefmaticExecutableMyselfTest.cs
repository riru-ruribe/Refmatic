using UnityEngine;
using Beryl.Refmatic;

namespace Beryl.Samples
{
    sealed class RefmaticExecutableMyselfTest : MonoBehaviour, IAutoRefmatic
    {
#pragma warning disable 414
        // use case 'RefmaticExecutableMyself' attribute.
        #region RefmaticExecutableMyself

        [SerializeField, RefmaticExecutableMyself]
        RectTransform rt = default;

        #endregion
#pragma warning restore 414
    }
}
