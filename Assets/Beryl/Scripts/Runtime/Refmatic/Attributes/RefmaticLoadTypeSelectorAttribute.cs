using System;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class RefmaticLoadTypeSelectorAttribute : Attribute
    {
        public IRefmaticLoadTypeSelector Selector { get; }

        public RefmaticLoadTypeSelectorAttribute(Type selectorType)
        {
            Selector = Activator.CreateInstance(selectorType) as IRefmaticLoadTypeSelector;
        }
    }
}
