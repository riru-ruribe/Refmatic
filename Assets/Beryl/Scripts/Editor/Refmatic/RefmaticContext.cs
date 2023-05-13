using UnityEditor;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    static class RefmaticContext
    {
        internal static void OnChanged<T>(Transform transform)
        {
            foreach (var cmp in transform.GetComponents<Component>())
                if (cmp is T)
                    Execute(cmp);
        }

        internal static void OnChangedInChild<T>(Transform transform)
        {
            var parent = transform.parent;
            while (parent != null)
            {
                OnChanged<T>(parent);
                parent = parent.parent;
            }
        }

        internal static void Execute(UnityEngine.Object obj)
        {
            // can not get private fields for parent classes. so get 'BaseType' and loop.
            var type = obj.GetType();
            if (RefmaticAttributeMetaPrefs.Instance.GetMap().TryGetValue(type.GetHashCode(), out RefmaticAttributeMetaPrefs.Script script))
            {
                for (int i = 0, len = script.fs.Count; i < len; i++)
                {
                    var f = script.fs[i];
                    if (f.bflg) type = type.BaseType;
                    f.e.Execute(obj, type.GetField(f.nm, RefmaticParams.bf));
                }
                LogExecute(obj);
                EditorUtility.SetDirty(obj);
            }
        }

        [System.Diagnostics.Conditional("LOG_ENABLED_REFMATIC")]
        static void LogExecute(UnityEngine.Object obj)
        {
#if UNITY_2021_2_OR_NEWER
            var sb = new UniSpanSb(2048);
            if (obj is Component cmp)
            {
                sb.NameWithParent(cmp.transform);
                sb.PrependColon();
                sb.Prepend(nameof(Execute));
                Debug.Log(sb.ResultWithDispose);
            }
            else
            {
                sb.Append(nameof(Execute));
                sb.AppendColon();
                sb.Append(obj.name);
                Debug.Log(sb.ResultWithDispose);
            }
#else
            Debug.Log($"Execute:{((obj is Component cmp) ? cmp.transform.NameWithParent() : obj.name)}");
#endif
        }

        [MenuItem("CONTEXT/MonoBehaviour/Refmatic Execute")]
        static void Execute(MenuCommand menuCommand)
        {
            Execute(menuCommand.context as UnityEngine.Object);
        }
    }
}
