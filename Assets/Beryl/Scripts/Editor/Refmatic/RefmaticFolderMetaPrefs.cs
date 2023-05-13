using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Beryl.Refmatic
{
    sealed class RefmaticFolderMetaPrefs : ScriptableObject
    {
        const string Path = "Assets/Beryl/Assets/Editor/RefmaticFolderMetaPrefs.asset";

        [Serializable]
        internal sealed class Folder
        {
            public string Name, LabelScenes, LabelObjects, SearchScenes, SearchObjects;
            public Item[] Scenes, Objects;

            [Serializable]
            internal sealed class Item
            {
                public string Path;
                public List<string> HierarchyPaths;
            }
        }

        [HideInInspector, SerializeField] List<Folder> folders = new List<Folder>();

        static RefmaticFolderMetaPrefs instance;
        internal static RefmaticFolderMetaPrefs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<RefmaticFolderMetaPrefs>(Path);
                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<RefmaticFolderMetaPrefs>();
                        AssetDatabase.CreateAsset(instance, Path);
                    }
                }
                return instance;
            }
        }

        internal Folder Get(string folderName)
        {
            foreach (var folder in folders)
            {
                if (string.Equals(folder.Name, folderName)) return folder;
            }
            return null;
        }

        internal void Save(string folderName, Folder.Item[] scenes, Folder.Item[] objects)
        {
            foreach (var folder in folders)
            {
                if (string.Equals(folder.Name, folderName))
                {
                    folder.Scenes = scenes ?? new Folder.Item[0];
                    folder.Objects = objects ?? new Folder.Item[0];
                    return;
                }
            }
            var newFolder = new Folder
            {
                Name = folderName,
                Scenes = scenes ?? new Folder.Item[0],
                Objects = objects ?? new Folder.Item[0],
            };
            folders.Add(newFolder);
        }

        internal void Remove(string folderName)
        {
            folders.RemoveAll(x => string.Equals(x.Name, folderName));
        }

        internal void Delete()
        {
            foreach (var folder in folders)
            {
                var importer = AssetImporter.GetAtPath(folder.Name);
                if (importer == null) continue;
                RefmaticEditorPrefs.AutoFolder.Remove(importer);
                importer.SaveAndReimport();
            }
            folders.Clear();
        }

        internal void Validate()
        {
            int idx = 0;
            while (idx < folders.Count)
            {
                var folder = folders[idx];
                var importer = AssetImporter.GetAtPath(folder.Name);
                // folder was deleted.
                if (importer == null)
                {
                    folders.Remove(folder);
                    continue;
                }
                idx++;
            }
        }
    }
}
