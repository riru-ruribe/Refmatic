using System;
using System.Collections;
using UnityEditor;

namespace Beryl.Util
{
    public sealed class EditorCoroutine : IDisposable
    {
        IEnumerator enumerator;
        public Action onCompleted;

        public EditorCoroutine(IEnumerator enumerator, Action onCompleted = null)
        {
            this.enumerator = enumerator;
            this.onCompleted = onCompleted;
            EditorApplication.update += Process;
        }

        void Process()
        {
            if (!enumerator.MoveNext())
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            EditorApplication.update -= Process;
            enumerator = null;
            onCompleted?.Invoke();
        }
    }
}
