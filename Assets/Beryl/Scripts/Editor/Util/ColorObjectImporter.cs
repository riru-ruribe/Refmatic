using System;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Beryl.Util
{
    [ScriptedImporter(version: 1, ext: "clo")]
    sealed class ColorObjectImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var color = Color.white;
            try
            {
                var text = File.ReadAllText(ctx.assetPath);
                if (!Validate(text, out color)) throw new FormatException();
            }
            catch (Exception exp)
            {
                Debug.LogError(exp);
            }

            var colorObject = ScriptableObject.CreateInstance<ColorObject>();
            colorObject.Color = color;

            ctx.AddObjectToAsset("ColorObject", colorObject);
            ctx.SetMainObject(colorObject);

            Debug.LogWarning($"ColorObjectImporter: {ctx.assetPath}");
        }

        internal static bool Validate(string text, out Color color)
        {
#if UNITY_2021_2_OR_NEWER
            if (text.StartsWith('#'))
#else
            if (text.StartsWith("#"))
#endif
            {
                return ColorUtility.TryParseHtmlString(text, out color);
            }
            else
            {
#if UNITY_2021_2_OR_NEWER
                ReadOnlySpan<char> span = text.AsSpan();
                char type;
                {
                    var indexOf = span.IndexOf(':');
                    type = span.Slice(0, indexOf)[0];
                    span = span.Slice(indexOf + 1);
                }
                switch (type)
                {
                    case 'b':
                        {
                            Color32 result = default;
                            for (int i = 0; i < 4; i++)
                            {
                                var indexOf = span.IndexOf(',');
                                if (indexOf == -1)
                                {
                                    result[i] = byte.Parse(span);
                                    break;
                                }
                                result[i] = byte.Parse(span.Slice(0, indexOf));
                                span = span.Slice(indexOf + 1);
                            }
                            color = result;
                            return true;
                        }
                    case 'f':
                        {
                            Color result = default;
                            for (int i = 0; i < 4; i++)
                            {
                                var indexOf = span.IndexOf(',');
                                if (indexOf == -1)
                                {
                                    result[i] = float.Parse(span);
                                    break;
                                }
                                result[i] = float.Parse(span.Slice(0, indexOf));
                                span = span.Slice(indexOf + 1);
                            }
                            color = result;
                            return true;
                        }
                }
#else
                var ary1 = text.Split(':');
                var ary2 = ary1[1].Split(',');
                switch (ary1[0])
                {
                    case "byte":
                        color = new Color32(
                            byte.Parse(ary2[0]),
                            byte.Parse(ary2[1]),
                            byte.Parse(ary2[2]),
                            byte.Parse(ary2[3])
                        );
                        return true;
                    case "float":
                        color = new Color(
                            float.Parse(ary2[0]),
                            float.Parse(ary2[1]),
                            float.Parse(ary2[2]),
                            float.Parse(ary2[3])
                        );
                        return true;
                }
#endif
            }
            color = default;
            return false;
        }
    }
}
