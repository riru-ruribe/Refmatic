namespace Beryl.Refmatic
{
    public interface IRefmaticElementLoad
    {
        string[] SearchInFolders { get; }
        bool IsMatch(string name);
    }
}
