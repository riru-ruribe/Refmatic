using UnityEngine;

namespace Beryl.Util
{
    /// <summary>
    /// treat a struct 'UnityEngine.Color' as if it were a 'UnityEngine.Object'.
    /// </summary>
    public sealed class ColorObject : ScriptableObject
    {
        public Color Color = default;
    }
}
