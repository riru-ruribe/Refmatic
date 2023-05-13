using UnityEditor;

namespace Beryl.Refmatic
{
    [InitializeOnLoad]
    static class RefmaticEditorPrefs
    {
        internal abstract class ModeBase
        {
            protected const string _Key = "__Beryl__Refmatic";
            protected const string _MenuPath = "Beryl/Refmatic/";
            protected static void _OnCheck(string key, string path)
            {
                var v = !Menu.GetChecked(path);
                EditorPrefs.SetBool(key, v);
                Menu.SetChecked(path, v);
            }
            protected static void _DelayCall(string key, string path)
            {
                Menu.SetChecked(path, EditorPrefs.GetBool(key, false));
            }
        }
        internal sealed class AutoMode : ModeBase
        {
            const string Key = _Key + "AutoMode";
            const string MenuPath = _MenuPath + "Flags/AutoRefmatic InHierarchy Enabled";
            internal static bool Has => EditorPrefs.GetBool(Key, false);
            [MenuItem(MenuPath, false, 0)] static void OnCheck() => _OnCheck(Key, MenuPath);
            internal static void DelayCall() => _DelayCall(Key, MenuPath);
        }
        internal sealed class AutoImportedMode : ModeBase
        {
            const string Key = _Key + "AutoImportedMode";
            const string MenuPath = _MenuPath + "Flags/AutoRefmatic Imported Enabled";
            internal static bool Has => EditorPrefs.GetBool(Key, false);
            [MenuItem(MenuPath, false, 1)] static void OnCheck() => _OnCheck(Key, MenuPath);
            internal static void DelayCall() => _DelayCall(Key, MenuPath);
        }
        sealed class AssetPickResolver : ModeBase
        {
            [MenuItem(_MenuPath + "Resolver/Update AssetPickResolver", false, 30)]
            static void Update() => RefmaticAssetPickResolver.Instance.PickAll();
        }
        sealed class GenericKeyResolver : ModeBase
        {
            [MenuItem(_MenuPath + "Resolver/Update GenericKeyResolver", false, 31)]
            static void Update() => RefmaticGenericKeyResolver.Instance.PickAll();
        }
        sealed class LoadTypeResolver : ModeBase
        {
            [MenuItem(_MenuPath + "Resolver/Update LoadTypeResolver", false, 32)]
            static void Update() => RefmaticLoadTypeResolver.Instance.PickAll();
        }
        sealed class SingleResolver : ModeBase
        {
            [MenuItem(_MenuPath + "Resolver/Update SingleResolver", false, 33)]
            static void Update() => RefmaticSingleResolver.Instance.PickAll();
        }
        sealed class TupleResolver : ModeBase
        {
            [MenuItem(_MenuPath + "Resolver/Update TupleResolver", false, 34)]
            static void Update() => RefmaticTupleResolver.Instance.PickAll();
        }
        sealed class AllResolver : ModeBase
        {
            [MenuItem(_MenuPath + "Resolver/Update All", false, 35)]
            static void Update()
            {
                RefmaticAssetPickResolver.Instance.PickAll();
                RefmaticGenericKeyResolver.Instance.PickAll();
                RefmaticLoadTypeResolver.Instance.PickAll();
                RefmaticSingleResolver.Instance.PickAll();
                RefmaticTupleResolver.Instance.PickAll();
            }
        }
        sealed class AttributeMetaPrefs : ModeBase
        {
            [MenuItem(_MenuPath + "Meta/Force Update AttributeMetaPrefs With Import", false, 96)]
            static void Update()
            {
                AssetDatabase.ImportAsset("Assets", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
            }
            [MenuItem(_MenuPath + "Meta/Delete AttributeMetaPrefs", false, 98)]
            static void Delete()
            {
                RefmaticAttributeMetaPrefs.Instance.Delete();
                EditorUtility.SetDirty(RefmaticAttributeMetaPrefs.Instance);
                AssetDatabase.SaveAssets();
            }
        }
        internal sealed class FolderMetaPrefs : ModeBase
        {
            [MenuItem(_MenuPath + "Meta/Validate FolderMetaPrefs", false, 97)]
            static void Update()
            {
                AssetDatabase.ImportAsset("Assets", ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
                RefmaticFolderMetaPrefs.Instance.Delete();
            }
            [MenuItem(_MenuPath + "Meta/Delete FolderMetaPrefs", false, 99)]
            static void Delete()
            {
                RefmaticFolderMetaPrefs.Instance.Delete();
                EditorUtility.SetDirty(RefmaticFolderMetaPrefs.Instance);
                AssetDatabase.SaveAssets();
            }
        }
        internal sealed class AutoFolder
        {
            const string Key = "AutoRefmatic";
            internal static bool Is(AssetImporter importer)
            {
                if (importer == null) return false;
                return importer.userData.Contains(Key);
            }
            internal static void Add(AssetImporter importer)
            {
                importer.userData += Key;
            }
            internal static void Remove(AssetImporter importer)
            {
                importer.userData = importer.userData.Replace(Key, "");
            }
        }

        static RefmaticEditorPrefs()
        {
            EditorApplication.delayCall += () =>
            {
                AutoMode.DelayCall();
                AutoImportedMode.DelayCall();
            };
        }
    }
}
