using Beryl.Util;

namespace Beryl.Refmatic
{
    static class RefmaticAttributeCollector
    {
        static ResizableArray<IRefmaticElementChild> _childs = new ResizableArray<IRefmaticElementChild>(8);
        static ResizableArray<IRefmaticElementLoad> _loads = new ResizableArray<IRefmaticElementLoad>(8);
        static ResizableArray<IRefmaticLoadTypeSelector> _lts = new ResizableArray<IRefmaticLoadTypeSelector>(8);

        public static IRefmaticElementChild[] childs;
        public static IRefmaticElementLoad[] loads;
        public static IRefmaticGenericKeySelector gk;
        public static IRefmaticLoadTypeSelector[] lts;
        public static IRefmaticSingleSelector sl1;
        public static IRefmaticTupleSelector sl2;

        public static void Collect(object[] atrs)
        {
            _childs.Clear();
            _loads.Clear();
            _lts.Clear();

            childs = null;
            loads = null;
            gk = null;
            lts = null;
            sl1 = null;
            sl2 = null;

            foreach (var atr in atrs)
            {
                switch (atr)
                {
                    case IRefmaticElementChild c:
                        _childs.Add(c);
                        break;
                    case IRefmaticElementLoad l:
                        _loads.Add(l);
                        break;
                    case RefmaticGenericKeySelectorAttribute gkatr:
                        gk = gkatr.Selector;
                        break;
                    case RefmaticLoadTypeSelectorAttribute ltatr:
                        _lts.Add(ltatr.Selector);
                        break;
                    case RefmaticSingleSelectorAttribute sl1atr:
                        sl1 = sl1atr.Selector;
                        break;
                    case RefmaticTupleSelectorAttribute sl2atr:
                        sl2 = sl2atr.Selector;
                        break;
                }
            }
            if (_childs.Length > 0) childs = _childs.ToArray();
            if (_loads.Length > 0) loads = _loads.ToArray();
            if (_lts.Length > 0) lts = _lts.ToArray();
        }
    }
}
