using System;

namespace Beryl.Refmatic
{
    /// <summary>
    /// resolve the difference between the class referenced by 'Component' and the class loaded from 'Assets' folder.<br/>
    /// default supports the following patterns.<br/>
    /// <see cref="RefmaticLoadTypeColorSelector"/><br/>
    /// ColorObject (.clo) -> UnityEngine.Color<br/>
    /// <see cref="RefmaticLoadTypeMonoScriptSelector"/><br/>
    /// UnityEditor.MonoScript -> RefmaticMonoScript<br/>
    /// <see cref="RefmaticLoadTypeDefaultSelector"/><br/>
    /// all other classes.
    /// </summary>
    public interface IRefmaticLoadTypeSelector : IRefmaticOrderable
    {
        Type ElementType { get; }
        Type ObjectType { get; }
        object Convert(Type elementType, UnityEngine.Object obj);
    }
}
