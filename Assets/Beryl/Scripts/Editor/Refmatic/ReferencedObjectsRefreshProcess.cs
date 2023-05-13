using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Beryl.Extensions;
using Beryl.Util;

namespace Beryl.Refmatic
{
    struct ReferencedObjectsRefreshProcess : IRefmaticCmpFindProcessor
    {
        public readonly struct SceneV : IRefmaticCmpFindValidator
        {
            static ResizableArray<string> _array = new ResizableArray<string>(256);
            readonly string[] guidAry;
            public bool ShouldFind => false;
            public bool ShouldValidate => true;
            public string[] ValidateGuids(string[] guids) => guidAry;
            public SceneV(RefmaticFolderMetaPrefs.Folder folder)
            {
                _array.Clear();
                for (int i = 0, len = folder.Scenes.Length; i < len; i++)
                    _array.Add(AssetDatabase.AssetPathToGUID(folder.Scenes[i].Path));
                guidAry = _array.ToArray();
            }
        }
        readonly struct Cache
        {
            public readonly Component Cmp;
            public readonly string HierarchyPath;
            public Cache(Component cmp, string hierarchyPath)
            {
                Cmp = cmp;
                HierarchyPath = hierarchyPath;
            }
        }

        readonly RefmaticFolderMetaPrefs.Folder folder;
        static HashSet<string> hierarchyPaths = new HashSet<string>();
        static List<Cache> caches = new List<Cache>();

        public void Pick(UnityEngine.Object obj, ref int picked)
        {
            if (obj is Component cmp)
            {
                var hierarchyPath = cmp.transform.NameWithParent();
                if (hierarchyPaths.Contains(hierarchyPath))
                {
                    caches.Add(new Cache(cmp, hierarchyPath));
                    picked++;
                }
            }
        }

        public EditorCoroutine OnClosingScene(ref UnityEngine.SceneManagement.Scene scene)
        {
            foreach (var item in folder.Scenes)
            {
                if (item.Path == scene.path)
                {
                    foreach (var cache in caches)
                    {
                        if (item.HierarchyPaths.Contains(cache.HierarchyPath))
                        {
                            if (PrefabUtility.IsPartOfAnyPrefab(cache.Cmp))
                            {
                                Debug.LogWarning($"skip because 'IsPartOfAnyPrefab'.\n{scene.name}.unity [{cache.HierarchyPath}]");
                                continue;
                            }
                            RefmaticContext.Execute(cache.Cmp);
                        }
                    }
                    EditorSceneManager.SaveScene(scene);
                    break;
                }
            }
            caches.Clear();
            return null;
        }

        public EditorCoroutine OnClosingObject(UnityEngine.Object obj, string path)
        {
            return null;
        }

        public void OnCompleted()
        {
        }

        public ReferencedObjectsRefreshProcess(RefmaticFolderMetaPrefs.Folder folder)
        {
            this.folder = folder;

            hierarchyPaths.Clear();
            for (int i = 0, len = folder.Scenes.Length; i < len; i++)
            {
                foreach (var path in folder.Scenes[i].HierarchyPaths)
                {
                    hierarchyPaths.Add(path);
                }
            }
        }
        public ReferencedObjectsRefreshProcess(RefmaticFolderMetaPrefs.Folder folder, HashSet<string> hierarchyPaths)
        {
            this.folder = folder;
            ReferencedObjectsRefreshProcess.hierarchyPaths = hierarchyPaths;
        }
    }
}
