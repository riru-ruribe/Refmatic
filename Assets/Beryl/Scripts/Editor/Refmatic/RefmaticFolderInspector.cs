using System.IO;
using UnityEditor;
using UnityEngine;

namespace Beryl.Refmatic
{
    [CustomEditor(typeof(DefaultAsset))]
    public class RefmaticFolderInspector : Editor
    {
        Vector2 pos1, pos2;

        public override void OnInspectorGUI()
        {
            var assetPath = AssetDatabase.GetAssetPath(target);

            if (!AssetDatabase.IsValidFolder(assetPath)) return;

            var importer = AssetImporter.GetAtPath(assetPath);

            GUI.enabled = true;
            EditorGUI.indentLevel++;
            {
                GUILayout.Space(20);

                var prev = RefmaticEditorPrefs.AutoFolder.Is(importer);
                var next = EditorGUILayout.Toggle("AutoRefmatic Imported", prev);
                if (next != prev)
                {
                    if (prev)
                    {
                        RefmaticEditorPrefs.AutoFolder.Remove(importer);
                        RefmaticFolderMetaPrefs.Instance.Remove(assetPath);
                    }
                    else
                    {
                        RefmaticEditorPrefs.AutoFolder.Add(importer);
                        RefmaticFolderMetaPrefs.Instance.Save(assetPath, null, null);
                    }
                    EditorUtility.SetDirty(RefmaticFolderMetaPrefs.Instance);
                    AssetDatabase.SaveAssets();
                    importer.SaveAndReimport();
                }

                EditorGUILayout.LabelField(assetPath);

                var folder = RefmaticFolderMetaPrefs.Instance.Get(assetPath);

                if (folder != null)
                {
                    folder.LabelScenes = EditorGUILayout.DelayedTextField("Label [Scenes]", folder.LabelScenes);
                    folder.LabelObjects = EditorGUILayout.DelayedTextField("Label [Objects]", folder.LabelObjects);
                    folder.SearchScenes = EditorGUILayout.DelayedTextField("SearchInFolders [Scenes]", folder.SearchScenes);
                    folder.SearchObjects = EditorGUILayout.DelayedTextField("SearchInFolders [Objects]", folder.SearchObjects);
                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(RefmaticFolderMetaPrefs.Instance);
                        AssetDatabase.SaveAssets();
                    }
                }

                EditorGUI.BeginDisabledGroup(!next);
                {
                    GUILayout.Space(20);

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("References Refresh", GUILayout.Width(160)))
                        RefreshReferences(assetPath);
                    if (GUILayout.Button("Referenced Objects Refresh", GUILayout.Width(200)))
                        RefreshObjects(assetPath);
                    GUILayout.EndHorizontal();

                    GUILayout.Space(20);

                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("References");

                    EditorGUI.indentLevel++;
                    if (folder != null)
                    {
                        EditorGUILayout.LabelField("Scenes");
                        {
                            pos1 = EditorGUILayout.BeginScrollView(pos1, GUI.skin.box, GUILayout.Height(200));
                            foreach (var scene in folder.Scenes)
                            {
                                EditorGUILayout.TextArea(scene.Path);
                                EditorGUI.indentLevel++;
                                foreach (var hp in scene.HierarchyPaths)
                                    EditorGUILayout.TextArea(hp);
                                EditorGUI.indentLevel--;
                            }
                            EditorGUILayout.EndScrollView();
                        }
                        EditorGUILayout.LabelField("Objects");
                        {
                            pos2 = EditorGUILayout.BeginScrollView(pos2, GUI.skin.box, GUILayout.Height(200));
                            foreach (var obj in folder.Objects)
                                EditorGUILayout.TextArea(obj.Path);
                            EditorGUILayout.EndScrollView();
                        }
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUI.indentLevel -= 3;
            GUI.enabled = false;
        }

        void RefreshReferences(string assetPath)
        {
            var referenced = LoadAssetsAtPath(assetPath);

            var map = RefmaticAttributeMetaPrefs.Instance.GetMap();

            var folder = RefmaticFolderMetaPrefs.Instance.Get(assetPath);
            RefmaticCmpFinderParams.SetSceneLabel(folder.LabelScenes);
            RefmaticCmpFinderParams.SetObjectLabel(folder.LabelObjects);
            RefmaticCmpFinderParams.SetScenePathsUnderAssets(folder.SearchScenes);
            RefmaticCmpFinderParams.SetObjectPathsUnderAssets(folder.SearchObjects);

            new RefmaticCmpFinder<FolderReferencesRefreshProcess>()
                .Run(
                    new FolderReferencesRefreshProcess(referenced, map, (scenes, objects) =>
                    {
                        RefmaticFolderMetaPrefs.Instance.Save(assetPath, scenes, objects);
                        EditorUtility.SetDirty(RefmaticFolderMetaPrefs.Instance);
                        AssetDatabase.SaveAssets();
                    }),
                    new DefaultRefmaticCmpFindValidator(shouldFind: true, shouldValidate: false),
                    new DefaultRefmaticCmpFindValidator(shouldFind: true, shouldValidate: false)
                );
        }

        UnityEngine.Object[] LoadAssetsAtPath(string assetPath)
        {
            RefmaticPools.Object.Clear();

            var paths = Directory.GetFiles(assetPath, "*", SearchOption.AllDirectories);
            foreach (var path in paths)
            {
                if (path.Contains(".meta")) continue;
                RefmaticPools.Object.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path));
            }

            return RefmaticPools.Object.ToArray();
        }

        void RefreshObjects(string assetPath)
        {
            var folder = RefmaticFolderMetaPrefs.Instance.Get(assetPath);
            if (folder == null) return;
            if (folder.Objects?.Length > 0)
            {
                foreach (var item in folder.Objects)
                {
                    var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(item.Path);
                    if (obj is GameObject go) RefmaticContext.OnChanged<IAutoRefmaticImported>(go.transform);
                    else RefmaticContext.Execute(obj);
                }
                AssetDatabase.SaveAssets();
            }
            if (folder.Scenes?.Length > 0)
            {
                RefmaticCmpFinderParams.SetSceneLabel(folder.LabelScenes);
                RefmaticCmpFinderParams.SetObjectLabel(folder.LabelObjects);
                RefmaticCmpFinderParams.SetScenePathsUnderAssets(folder.SearchScenes);
                RefmaticCmpFinderParams.SetObjectPathsUnderAssets(folder.SearchObjects);
                new RefmaticCmpFinder<ReferencedObjectsRefreshProcess>()
                    .Run(
                        new ReferencedObjectsRefreshProcess(folder),
                        new ReferencedObjectsRefreshProcess.SceneV(folder),
                        new DefaultRefmaticCmpFindValidator(shouldFind: false, shouldValidate: false)
                    );
            }
        }
    }
}
