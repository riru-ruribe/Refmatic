using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Beryl.Util
{
    [CustomEditor(typeof(ColorObjectImporter))]
    sealed class ColorObjectImporterEditor : ScriptedImporterEditor
    {
        protected override bool needsApplyRevert => false;

        public override void OnInspectorGUI()
        {
            var importer = target as AssetImporter;

            var src = File.ReadAllText(importer.assetPath);
            var dst = EditorGUILayout.DelayedTextField(src);
            if (GUI.changed)
            {
                if (ColorObjectImporter.Validate(dst, out _))
                {
                    File.WriteAllText(importer.assetPath, dst);
                    importer.SaveAndReimport();
                }
            }
        }
    }
}
