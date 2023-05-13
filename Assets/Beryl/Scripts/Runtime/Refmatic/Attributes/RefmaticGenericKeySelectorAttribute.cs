using System;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RefmaticGenericKeySelectorAttribute : Attribute
    {
        public IRefmaticGenericKeySelector Selector { get; }

        public RefmaticGenericKeySelectorAttribute(Type selectorType)
        {
            Selector = Activator.CreateInstance(selectorType) as IRefmaticGenericKeySelector;
        }
    }
}
