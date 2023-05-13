using System;
using Beryl.Util;

namespace Beryl.Refmatic
{
    /// <summary>
    /// can change the loading format of your assets.<br/>
    /// default supports the following patterns.<br/>
    /// <see cref="RefmaticAssetPickSpriteSelector"/><br/>
    /// <see cref="RefmaticAssetPickDefaultSelector"/>
    /// </summary>
    public interface IRefmaticAssetPickSelector : IRefmaticOrderable
    {
        Type Type { get; }
        void PickAsset(Type type, string path, IRefmaticElementLoad element, ref ResizableArray<UnityEngine.Object> array);
    }
}
