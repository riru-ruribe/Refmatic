namespace Beryl.Refmatic
{
    sealed class RefmaticNameStartsWith : IRefmaticNameComparison
    {
        public bool IsMatch(string name, string key)
        {
#if UNITY_2021_2_OR_NEWER
            return System.MemoryExtensions.StartsWith<char>(name, key);
#else
            return name.StartsWith(key);
#endif
        }
    }
}
