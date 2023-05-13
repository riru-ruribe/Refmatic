namespace Beryl.Refmatic
{
    sealed class RefmaticNameEquals : IRefmaticNameComparison
    {
        public bool IsMatch(string name, string key)
        {
            return string.Equals(name, key);
        }
    }
}
