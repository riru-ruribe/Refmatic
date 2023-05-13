using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Beryl.Util;

namespace Beryl.Refmatic
{
    sealed class RefmaticCmpFinder<T> : IDisposable where T : IRefmaticCmpFindProcessor
    {
        static List<GameObject> rootGos = new List<GameObject>();
        EditorCoroutine coroutine;
        T processor;
        string dispName;
        bool wait, running;
        int index, count;
        string[] scenes, objects;

        public void Run<TV1, TV2>(T processor, TV1 sceneV, TV2 objectV, string dispName = null)
            where TV1 : IRefmaticCmpFindValidator
            where TV2 : IRefmaticCmpFindValidator
        {
            this.dispName = dispName ?? typeof(T).Name;
            this.processor = processor;
            scenes = ValidateGuids<TV1>(sceneV,
                RefmaticCmpFinderParams.Key1 + RefmaticCmpFinderParams.SceneLabel,
                RefmaticCmpFinderParams.ScenePathsUnderAssets);
            objects = ValidateGuids<TV2>(objectV,
                RefmaticCmpFinderParams.Key2 + RefmaticCmpFinderParams.ObjectLabel,
                RefmaticCmpFinderParams.ObjectPathsUnderAssets);
            count = (scenes?.Length ?? 0) + (objects?.Length ?? 0);
            if (count <= 0)
            {
                Debug.LogWarning("not find assets.");
                processor.OnCompleted();
                return;
            }
            running = true;
            EditorUtility.DisplayCancelableProgressBar(dispName, null, (float)index++ / count);
            EditorSceneManager.sceneOpened += OnOpendScene;
            coroutine = new EditorCoroutine(Co(), () =>
            {
                running = false;
                processor.OnCompleted();
                EditorUtility.ClearProgressBar();
            });
        }

        string[] ValidateGuids<TV>(TV validator, string key, string[] folders)
            where TV : IRefmaticCmpFindValidator
        {
            if (validator.ShouldValidate)
                return validator.ValidateGuids(validator.ShouldFind ? FindAssets(key, folders) : null);
            return validator.ShouldFind ? FindAssets(key, folders) : null;
        }

        string[] FindAssets(string filter, string[] folders)
        {
            if (folders?.Length > 0)
            {
                return AssetDatabase.FindAssets(filter, folders);
            }
            return AssetDatabase.FindAssets(filter);
        }

        IEnumerator Co()
        {
            if (scenes != null)
            {
                foreach (var guid in scenes)
                {
                    wait = true;
                    EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(guid));
                    while (wait) yield return null;
                    EditorUtility.DisplayCancelableProgressBar(dispName, null, (float)index++ / count);
                }
            }
            EditorSceneManager.sceneOpened -= OnOpendScene;

            if (objects != null)
            {
                foreach (var guid in objects)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                    int picked = 0;
                    if (obj is GameObject go)
                    {
                        foreach (var cmp in go.GetComponentsInChildren<IAutoRefmaticPickable>())
                            processor.Pick(cmp as Component, ref picked);
                    }
                    else
                    {
                        processor.Pick(obj, ref picked);
                    }
                    if (picked > 0)
                    {
                        var _co = processor.OnClosingObject(obj, path);
                        if (_co != null)
                        {
                            var w = true;
                            _co.onCompleted += () => w = false;
                            while (w) yield return null;
                        }
                    }

                    EditorUtility.DisplayCancelableProgressBar(dispName, null, (float)index++ / count);
                }
            }
        }

        void OnOpendScene(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            int picked = 0;
            scene.GetRootGameObjects(rootGos);
            foreach (var go in rootGos)
                foreach (var cmp in go.GetComponentsInChildren<IAutoRefmaticPickable>())
                    processor.Pick(cmp as Component, ref picked);
            if (picked > 0)
            {
                var _co = processor.OnClosingScene(ref scene);
                if (_co != null)
                {
                    _co.onCompleted += () => wait = false;
                    return;
                }
            }
            wait = false;
        }

        public void Dispose()
        {
            if (running)
            {
                running = false;
                if (coroutine != null)
                {
                    coroutine.onCompleted = null;
                    coroutine.Dispose();
                }
                EditorSceneManager.sceneOpened -= OnOpendScene;
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
