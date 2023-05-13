using System;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RefmaticSingleSelectorAttribute : Attribute
    {
        public IRefmaticSingleSelector Selector { get; }

        public RefmaticSingleSelectorAttribute(Type selectorType)
        {
            Selector = Activator.CreateInstance(selectorType) as IRefmaticSingleSelector;
        }
    }
}
