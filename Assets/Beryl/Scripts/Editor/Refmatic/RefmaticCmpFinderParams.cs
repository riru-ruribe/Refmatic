using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Beryl.Refmatic
{
    sealed class RefmaticCmpFinderParams : ScriptableObject
    {
        const string Path = "Assets/Beryl/Assets/Editor/RefmaticCmpFinderParams.asset";

        [SerializeField] string defaultObjectFilter = "t:Prefab,t:ScriptableObject";

        static RefmaticCmpFinderParams instance;
        static RefmaticCmpFinderParams Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<RefmaticCmpFinderParams>(Path);
                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<RefmaticCmpFinderParams>();
                        AssetDatabase.CreateAsset(instance, Path);
                    }
                }
                return instance;
            }
        }

        public const string Key1 = "t:Scene";
        public static string Key2 => Instance.defaultObjectFilter;
        public static string SceneLabel, ObjectLabel;
        public static string[] ScenePathsUnderAssets, ObjectPathsUnderAssets;
        public static void SetSceneLabel(string str) => _SetLabel(ref SceneLabel, str);
        public static void SetObjectLabel(string str) => _SetLabel(ref ObjectLabel, str);
        static void _SetLabel(ref string self, string str)
        {
            self = string.IsNullOrEmpty(str) ? null : str.Contains("l:") ? $" {str}" : $" l:{str}";
        }
        public static void SetScenePathsUnderAssets(string str) => _SetPathsUnderAssets(ref ScenePathsUnderAssets, str);
        public static void SetObjectPathsUnderAssets(string str) => _SetPathsUnderAssets(ref ObjectPathsUnderAssets, str);
        static void _SetPathsUnderAssets(ref string[] self, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                self = null;
                return;
            }
#if UNITY_2021_2_OR_NEWER
            if (str.Contains(','))
#else
            if (str.Contains(","))
#endif
            {
                self = str.Split(',').Select(x => x.Trim()).ToArray();
                return;
            }
            self = new[] { str.Trim(), };
        }
    }
}
