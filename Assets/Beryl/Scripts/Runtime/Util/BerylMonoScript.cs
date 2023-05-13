using System;

namespace Beryl.Util
{
    /// <summary>
    /// wrapper class for dynamically creating a script instance (on editor).<br/>
    /// not worry about compile errors.
    /// </summary>
    public readonly struct BerylMonoScript
    {
#if UNITY_EDITOR
        readonly UnityEditor.MonoScript monoScript;
#endif
        public Type GetClass()
        {
#if UNITY_EDITOR
            return monoScript.GetClass();
#else
            return null;
#endif
        }
        public object CreateInstance()
        {
            return Activator.CreateInstance(GetClass());
        }
        public object CreateInstance(params object[] args)
        {
            return Activator.CreateInstance(GetClass(), args);
        }
        public BerylMonoScript(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            monoScript = obj as UnityEditor.MonoScript;
#endif
        }
    }
}
