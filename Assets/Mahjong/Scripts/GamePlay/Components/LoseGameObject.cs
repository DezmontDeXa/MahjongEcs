using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace DDX
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct LoseGameObject : IComponent
    {
        public bool Shown;
    }
}