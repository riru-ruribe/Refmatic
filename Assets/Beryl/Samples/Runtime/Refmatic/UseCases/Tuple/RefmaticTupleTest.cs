using System;
using UnityEngine;
using Beryl.Refmatic;
using Beryl.Util;

namespace Beryl.Samples
{
    // use case 'RefmaticTupleSelector' attribute.
    //   when the referenced field class itself is of its own types.
    //   when you want to refer to multiple objects other than 'SerializableTuple'.
    sealed class RefmaticTupleTest : MonoBehaviour, IAutoRefmaticImported
    {
#pragma warning disable 414
        const string Root = "Assets/Beryl/Samples/Runtime/Refmatic/";

        // implementation should be a class because reference using 'SerializeReference'.
        sealed class Selector : IRefmaticTupleSelector
        {
            public int Order => 0;
            public Type[] MatchType(Type type, out Type keyType)
            {
                keyType = null;
                return new[] { typeof(Sprite), typeof(BerylMonoScript), };
            }
            public object Select(Type type, IRefmaticGenericKeySelector gk, IRefmaticLoadTypeSelector[] lts, UnityEngine.Object[] objs)
            {
                if (objs[0] == null || objs[1] == null)
                    return null;
                return new BerylMonoScript(objs[1]).CreateInstance(new object[] { objs[0], });
            }
        }

        [RefmaticTupleSelector(typeof(Selector))]
        [RefmaticElementLoad(@"^log_img_\d{2,}", RefmaticComparisons.Regex, Root + "Loggers/")]
        [RefmaticElementLoad(@"^TestLogger\d{1,}", RefmaticComparisons.Regex, Root + "Loggers/")]
        [SerializeReference, RefmaticExecutableLoad]
        ITestLogger[] loggers = default;

        [ContextMenu("Log")]
        void Log()
        {
            foreach (var logger in loggers)
                logger.Log(transform);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

#pragma warning restore 414
    }
}
