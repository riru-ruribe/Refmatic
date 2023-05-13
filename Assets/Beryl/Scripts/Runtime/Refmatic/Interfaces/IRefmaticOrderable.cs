namespace Beryl.Refmatic
{
    /// <summary>
    /// determine the priority of processing.
    /// </summary>
    public interface IRefmaticOrderable
    {
        int Order { get; }
    }
}
