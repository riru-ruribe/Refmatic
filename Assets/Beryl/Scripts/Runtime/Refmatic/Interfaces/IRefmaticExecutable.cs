using System.Reflection;

namespace Beryl.Refmatic
{
    public interface IRefmaticExecutable
    {
        void Prepare(
            FieldInfo fi,
            IRefmaticElementChild[] childs,
            IRefmaticElementLoad[] loads,
            IRefmaticGenericKeySelector gk,
            IRefmaticLoadTypeSelector[] lts,
            IRefmaticSingleSelector sl1,
            IRefmaticTupleSelector sl2);
        void Execute(UnityEngine.Object obj, FieldInfo fi);
        bool Referenceable(UnityEngine.Object[] objs);
    }
}
