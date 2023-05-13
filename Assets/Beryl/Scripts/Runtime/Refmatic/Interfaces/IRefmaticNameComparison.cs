namespace Beryl.Refmatic
{
    /// <summary>
    /// implement a name lookup method.
    /// default supports the following patterns.<br/>
    /// <see cref="RefmaticNameEquals"/><br/>
    /// <see cref="RefmaticNameStartsWith"/><br/>
    /// <see cref="RefmaticNameEndsWith"/><br/>
    /// <see cref="RefmaticNameRegex"/>
    /// </summary>
    public interface IRefmaticNameComparison
    {
        bool IsMatch(string name, string key);
    }
}
