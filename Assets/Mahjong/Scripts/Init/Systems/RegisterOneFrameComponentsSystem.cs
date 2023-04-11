using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers.OneFrame;

namespace DDX
{

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(RegisterOneFrameComponentsSystem))]
    public sealed class RegisterOneFrameComponentsSystem : Initializer
    {
        public override void OnAwake()
        {
            World.RegisterOneFrame<DicePointerEnterEvent>();
            World.RegisterOneFrame<DicePointerExitEvent>();
            World.RegisterOneFrame<DiceClickedEvent>();
            World.RegisterOneFrame<StartGameEvent>();
        }

        public override void Dispose()
        {
        }
    }
}