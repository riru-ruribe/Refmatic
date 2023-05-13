using Beryl.Util;

namespace Beryl.Refmatic
{
    interface IRefmaticCmpFindProcessor
    {
        void Pick(UnityEngine.Object obj, ref int picked);
        EditorCoroutine OnClosingScene(ref UnityEngine.SceneManagement.Scene scene);
        EditorCoroutine OnClosingObject(UnityEngine.Object obj, string path);
        void OnCompleted();
    }

    interface IRefmaticCmpFindValidator
    {
        bool ShouldFind { get; }
        bool ShouldValidate { get; }
        string[] ValidateGuids(string[] guids);
    }

    readonly struct DefaultRefmaticCmpFindValidator : IRefmaticCmpFindValidator
    {
        readonly bool shouldFind, shouldValidate;
        public bool ShouldFind => shouldFind;
        public bool ShouldValidate => shouldValidate;
        public string[] ValidateGuids(string[] guids) => guids;
        public DefaultRefmaticCmpFindValidator(bool shouldFind, bool shouldValidate)
        {
            this.shouldFind = shouldFind;
            this.shouldValidate = shouldValidate;
        }
    }
}
