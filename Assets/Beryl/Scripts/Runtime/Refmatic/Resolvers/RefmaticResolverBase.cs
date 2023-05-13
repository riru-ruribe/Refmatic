using System;
using System.Linq;
using UnityEngine;

namespace Beryl.Refmatic
{
    public abstract class RefmaticResolverBase<TObj, TVal> : ScriptableObject
        where TObj : RefmaticResolverBase<TObj, TVal>
        where TVal : class, IRefmaticOrderable
    {
        [SerializeReference] internal TVal[] values = default;

        static TObj instance;
        public static TObj Instance
        {
            get
            {
                if (instance == null)
                {
#if UNITY_EDITOR
                    instance = ScriptableObject.CreateInstance<TObj>();
                    instance = UnityEditor.AssetDatabase.LoadAssetAtPath<TObj>(instance.Path);
                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<TObj>();
                        UnityEditor.AssetDatabase.CreateAsset(instance, instance.Path);
                    }
                    if (instance.values == null || instance.values.Length <= 0)
                        instance.PickAll();
#else
                    instance = ScriptableObject.CreateInstance<TObj>();
#endif
                }
                return instance;
            }
        }

        protected abstract string Path { get; }

        public void PickAll()
        {
#if UNITY_EDITOR
            var baseType = typeof(TVal);
            values = UnityEditor.AssetDatabase.FindAssets("t:script", new[] { "Assets", })
                .Select(guid => UnityEditor.AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.MonoScript>(path).GetClass())
                .Where(x => x != null && x != baseType) // ignore myself.
                .Where(x => !x.IsAbstract)
                .Where(x => x.GetInterfaces().Any(y => y == baseType))
                .Select(x => Activator.CreateInstance(x) as TVal)
                .OrderBy(x => x.Order)
                .ToArray();
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}
