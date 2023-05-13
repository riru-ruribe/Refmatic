using System;
using UnityEngine;
using Beryl.Refmatic;
using Beryl.Util;

namespace Beryl.Samples
{
    // use case 'RefmaticLoadTypeSelector' attribute.
    // when you want to convert the obtained references to your own types.
    sealed class RefmaticLoadTypeTest : MonoBehaviour, IAutoRefmaticImported
    {
#pragma warning disable 414
        const string Root = "Assets/Beryl/Samples/Runtime/Refmatic/";

        // implementation should be a class because reference using 'SerializeReference'.
        sealed class Selector : IRefmaticLoadTypeSelector
        {
            public int Order => 0;
            public Type ElementType => typeof(Color);
            public Type ObjectType => typeof(ColorObject);
            public object Convert(Type type, UnityEngine.Object obj)
            {
                return (obj as ColorObject)?.Color ?? Color.white;
            }
        }

        [RefmaticLoadTypeSelector(typeof(Selector))]
        [RefmaticElementLoad(@"^beryl_clr_\d{2,}", RefmaticComparisons.Regex, Root + "Colors/")]
        [SerializeReference, RefmaticExecutableLoad]
        Color[] colors = default;

#pragma warning restore 414
    }
}
