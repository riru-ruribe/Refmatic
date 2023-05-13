using System.Collections.Generic;
using UnityEngine;
using Beryl.Extensions;
using Beryl.Util;

namespace Beryl.Refmatic
{
    using FolderItem = RefmaticFolderMetaPrefs.Folder.Item;
    using AtrMetaMap = Dictionary<int, RefmaticAttributeMetaPrefs.Script>;

    struct FolderReferencesRefreshProcess : IRefmaticCmpFindProcessor
    {
        public delegate void OnCompletedDlg(FolderItem[] scenes, FolderItem[] objects);

        readonly UnityEngine.Object[] referenced;
        readonly AtrMetaMap atrMetaMap;
        readonly OnCompletedDlg onCompletedDlg;
        static ResizableArray<FolderItem> scenes = new ResizableArray<FolderItem>(256);
        static ResizableArray<FolderItem> objects = new ResizableArray<FolderItem>(256);
        static HashSet<UnityEngine.Object> hashSet = new HashSet<UnityEngine.Object>();

        public void Pick(UnityEngine.Object obj, ref int picked)
        {
            if (atrMetaMap.TryGetValue(obj.GetType().GetHashCode(), out RefmaticAttributeMetaPrefs.Script script))
            {
                for (int i = 0, len = script.fs.Count; i < len; i++)
                {
                    if (script.fs[i].e.Referenceable(referenced))
                    {
                        if (hashSet.Add(obj)) picked++;
                        return;
                    }
                }
            }
        }

        public EditorCoroutine OnClosingScene(ref UnityEngine.SceneManagement.Scene scene)
        {
            if (hashSet.Count > 0)
            {
                var folderItem = new FolderItem
                {
                    Path = scene.path,
                    HierarchyPaths = new List<string>(hashSet.Count),
                };
                foreach (var obj in hashSet)
                    folderItem.HierarchyPaths.Add((obj as Component).transform.NameWithParent());
                scenes.Add(folderItem);
            }
            hashSet.Clear();
            return null;
        }

        public EditorCoroutine OnClosingObject(UnityEngine.Object obj, string path)
        {
            if (hashSet.Count > 0)
                objects.Add(new FolderItem { Path = path, });
            hashSet.Clear();
            return null;
        }

        public void OnCompleted()
        {
            onCompletedDlg(scenes.ToArray(), objects.ToArray());
        }

        public FolderReferencesRefreshProcess(UnityEngine.Object[] referenced, AtrMetaMap atrMetaMap, OnCompletedDlg onCompletedDlg)
        {
            this.referenced = referenced;
            this.atrMetaMap = atrMetaMap;
            this.onCompletedDlg = onCompletedDlg;
            scenes.Clear();
            objects.Clear();
        }
    }
}
