namespace Beryl.Refmatic
{
    /// <summary>
    /// if inherit, it will be automatically updated
    /// when assets are added or removed from the 'AutoRefmatic Imported' flagged folder.<br/>
    /// also, you have to enable the event itself from the menu below.<br/>
    /// Beryl > Refmatic > Flags > AutoRefmatic Imported Enabled ☑︎
    /// </summary>
    public interface IAutoRefmaticImported : IAutoRefmaticPickable
    {
    }
}
