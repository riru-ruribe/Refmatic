namespace Beryl.Refmatic
{
    sealed class RefmaticNameEndsWith : IRefmaticNameComparison
    {
        public bool IsMatch(string name, string key)
        {
#if UNITY_2021_2_OR_NEWER
            return System.MemoryExtensions.EndsWith<char>(name, key);
#else
            return name.EndsWith(key);
#endif
        }
    }
}
