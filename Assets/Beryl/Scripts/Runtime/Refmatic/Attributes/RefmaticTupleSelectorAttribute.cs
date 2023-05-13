using System;

namespace Beryl.Refmatic
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RefmaticTupleSelectorAttribute : Attribute
    {
        public IRefmaticTupleSelector Selector { get; }

        public RefmaticTupleSelectorAttribute(Type selectorType)
        {
            Selector = Activator.CreateInstance(selectorType) as IRefmaticTupleSelector;
        }
    }
}
