using System.Reflection;

namespace Beryl.Refmatic
{
    public interface IRefmaticExecutor
    {
        void Execute(UnityEngine.Object obj, FieldInfo fi);
        bool Referenceable(UnityEngine.Object[] objs);
    }
}
