using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Beryl.Refmatic
{
    sealed class RefmaticAttributeMetaPrefs : ScriptableObject
    {
        const string Path = "Assets/Beryl/Assets/Editor/RefmaticAttributeMetaPrefs.asset";

        [Serializable]
        internal sealed class Script
        {
            public string t, p;
            public List<Field> fs;

            [Serializable]
            internal sealed class Field
            {
                public string nm;
                public int bidx; // index to parent script. role in minimizing the use of 'Reflection'.
                public bool bflg;
                [SerializeReference] public IRefmaticExecutable e;
            }

            internal Script(Type type, List<Field> fs, string assetPath)
            {
                this.t = GetTypeName(type);
                this.fs = fs;
                this.p = assetPath;
            }
        }

        [HideInInspector, SerializeField] List<Script> scripts = new List<Script>();

        Dictionary<int, Script> map;

        static RefmaticAttributeMetaPrefs instance;
        internal static RefmaticAttributeMetaPrefs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<RefmaticAttributeMetaPrefs>(Path);
                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<RefmaticAttributeMetaPrefs>();
                        AssetDatabase.CreateAsset(instance, Path);
                    }
                    instance.ToMap();
                }
                return instance;
            }
        }

        internal bool TryGetWithPath(string assetPath, out Script script)
        {
            for (int i = 0, len = scripts.Count; i < len; i++)
            {
                script = scripts[i];
                if (string.Equals(script.p, assetPath)) return true;
            }
            script = null;
            return false;
        }

        internal void Add(Script script)
        {
            scripts.Add(script);
        }

        internal int Remove(string assetPath)
        {
            return scripts.RemoveAll(x => string.Equals(x.p, assetPath));
        }

        internal void Delete()
        {
            scripts.Clear();
            map = null;
        }

        internal void ToMap(bool force = true)
        {
            if (force || map == null)
            {
                // there is a script file, but the 'Type' may not be obtained due to commenting out etc.
                map = scripts
                    .Select(x => (Type.GetType(x.t)?.GetHashCode(), x))
                    .Where(x => x.Item1 != null)
                    .ToDictionary(x => x.Item1.Value, x => x.Item2);
            }
        }

        internal Dictionary<int, Script> GetMap()
        {
            ToMap(force: false);
            return map;
        }

        static string GetTypeName(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }
    }
}
