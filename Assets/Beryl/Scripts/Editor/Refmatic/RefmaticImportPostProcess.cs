using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Beryl.Refmatic
{
    using FolderPrefs = RefmaticFolderMetaPrefs;
    using AtrPrefs = RefmaticAttributeMetaPrefs;

    sealed class RefmaticImportPostProcess : AssetPostprocessor
    {
        const string ScriptExtension = ".cs";
        const string IndexOfValue = "Assets/";
        static HashSet<FolderPrefs.Folder> folders = new HashSet<FolderPrefs.Folder>();
        static Dictionary<string, HashSet<string>> sceneHash = new Dictionary<string, HashSet<string>>();

        static bool IsAutoFolder(string assetPath, out string path)
        {
            path = null;
            if (string.IsNullOrEmpty(assetPath)) return false;
            var di = Directory.GetParent(assetPath);
            if (di == null) return false;
            var indexOf = di.FullName.IndexOf(IndexOfValue);
            if (indexOf > 0) path = di.FullName.Substring(indexOf);
            return RefmaticEditorPrefs.AutoFolder.Is(AssetImporter.GetAtPath(path));
        }

        // TODO: consider optimization based on future trends.
        static bool UpdateAttributePrefs(string assetPath)
        {
            var classType = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath)?.GetClass();
            if (classType == null) return false;

            List<AtrPrefs.Script.Field> scriptFields = null;

            int baseIndex = 0;
            bool baseFlg = false;
            // can not get private fields for parent classes. so get 'BaseType' and loop.
            var baseType = classType;
            while (baseType != null)
            {
                var fis = baseType.GetFields(RefmaticParams.bf);
                for (int i = 0, len1 = fis.Length; i < len1; i++)
                {
                    var fi = fis[i];
                    var atrs = fi.GetCustomAttributes(false);
                    for (int j = 0, len2 = atrs.Length; j < len2; j++)
                    {
                        if (atrs[j] is IRefmaticExecutable e)
                        {
                            RefmaticAttributeCollector.Collect(atrs);
                            e.Prepare(fi,
                                RefmaticAttributeCollector.childs,
                                RefmaticAttributeCollector.loads,
                                RefmaticAttributeCollector.gk,
                                RefmaticAttributeCollector.lts,
                                RefmaticAttributeCollector.sl1,
                                RefmaticAttributeCollector.sl2);
                            scriptFields ??= new List<AtrPrefs.Script.Field>();
                            scriptFields.Add(new AtrPrefs.Script.Field
                            {
                                nm = fi.Name,
                                bidx = baseIndex,
                                bflg = baseFlg,
                                e = e,
                            });
                            baseFlg = false;
                        }
                    }
                }
                baseIndex++;
                baseFlg = true;
                baseType = baseType.BaseType;
            }

            var isDirty = scriptFields != null;
            if (isDirty)
            {
                if (AtrPrefs.Instance.TryGetWithPath(assetPath, out AtrPrefs.Script script))
                    script.fs = scriptFields;
                else
                    AtrPrefs.Instance.Add(new AtrPrefs.Script(classType, scriptFields, assetPath));
            }
            return isDirty;
        }

        static bool IsScript(string assetPath)
        {
#if UNITY_2021_2_OR_NEWER
            return System.MemoryExtensions.EndsWith<char>(assetPath, ScriptExtension);
#else
            return assetPath.EndsWith(ScriptExtension);
#endif
        }

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            bool isDirtyAttributeMetaPrefs = false;

            folders.Clear();

            for (int i = 0, len = importedAssets.Length; i < len; i++)
            {
                var assetPath = importedAssets[i];
                if (IsScript(assetPath))
                {
                    isDirtyAttributeMetaPrefs |= UpdateAttributePrefs(assetPath);
                }
                else if (IsAutoFolder(assetPath, out string path))
                {
                    var folder = FolderPrefs.Instance.Get(path);
                    if (folder == null) continue;
                    folders.Add(folder);
                }
            }
            for (int i = 0, len = deletedAssets.Length; i < len; i++)
            {
                var assetPath = deletedAssets[i];
                if (IsScript(assetPath))
                {
                    isDirtyAttributeMetaPrefs |= AtrPrefs.Instance.Remove(assetPath) > 0;
                }
                else if (IsAutoFolder(assetPath, out string path))
                {
                    var folder = FolderPrefs.Instance.Get(path);
                    if (folder == null) continue;
                    folders.Add(folder);
                    break;
                }
            }
            for (int i = 0, len = movedFromAssetPaths.Length; i < len; i++)
            {
                var assetPath = movedFromAssetPaths[i];
                if (IsScript(assetPath))
                {
                    isDirtyAttributeMetaPrefs |= AtrPrefs.Instance.Remove(assetPath) > 0;
                }
                else if (IsAutoFolder(assetPath, out string path))
                {
                    var folder = FolderPrefs.Instance.Get(path);
                    if (folder == null) continue;
                    folders.Add(folder);
                }
            }
            for (int i = 0, len = movedAssets.Length; i < len; i++)
            {
                var assetPath = movedAssets[i];
                if (IsScript(assetPath))
                {
                    isDirtyAttributeMetaPrefs |= UpdateAttributePrefs(assetPath);
                }
                else if (IsAutoFolder(assetPath, out string path))
                {
                    var folder = FolderPrefs.Instance.Get(path);
                    if (folder == null) continue;
                    folders.Add(folder);
                }
            }

            if (isDirtyAttributeMetaPrefs)
            {
                EditorUtility.SetDirty(AtrPrefs.Instance);
                AssetDatabase.SaveAssets();
                AtrPrefs.Instance.ToMap();
            }

            if (!RefmaticEditorPrefs.AutoImportedMode.Has) return;

            if (folders.Count > 0)
            {
                sceneHash.Clear();
                foreach (var folder in folders)
                {
                    var items = folder.Objects;
                    if (items?.Length > 0)
                    {
                        foreach (var item in items)
                        {
                            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(item.Path);
                            if (obj is GameObject go) RefmaticContext.OnChanged<IAutoRefmaticImported>(go.transform);
                            else RefmaticContext.Execute(obj);
                        }
                    }
                    items = folder.Scenes;
                    if (items?.Length > 0)
                    {
                        foreach (var item in items)
                        {
                            if (!sceneHash.ContainsKey(item.Path))
                                sceneHash.Add(item.Path, new HashSet<string>());
                            foreach (var hp in item.HierarchyPaths)
                                sceneHash[item.Path].Add(hp);
                        }
                    }
                }
                folders.Clear();
                AssetDatabase.SaveAssets();

                if (sceneHash.Count > 0)
                {
                    var folder = new FolderPrefs.Folder
                    {
                        Scenes = sceneHash
                            .Select(x => new FolderPrefs.Folder.Item
                            {
                                Path = x.Key,
                                HierarchyPaths = x.Value.ToList(),
                            })
                            .ToArray()
                    };
                    var hierarchyPaths = new HashSet<string>(sceneHash.Values.SelectMany(x => x));
                    RefmaticCmpFinderParams.SetSceneLabel(null);
                    RefmaticCmpFinderParams.SetObjectLabel(null);
                    RefmaticCmpFinderParams.SetScenePathsUnderAssets(null);
                    RefmaticCmpFinderParams.SetObjectPathsUnderAssets(null);
                    new RefmaticCmpFinder<ReferencedObjectsRefreshProcess>()
                        .Run(
                            new ReferencedObjectsRefreshProcess(folder, hierarchyPaths),
                            new ReferencedObjectsRefreshProcess.SceneV(folder),
                            new DefaultRefmaticCmpFindValidator(shouldFind: false, shouldValidate: false)
                        );
                }
                sceneHash.Clear();
            }
        }
    }
}
