using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers.OneFrame;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DiceClickedListenerSystem))]
    public sealed class DiceClickedListenerSystem : SimpleUpdateSystem<Dice, DiceClickedEvent, DiceCanSelectTag>
    {
        public override void OnAwake()
        {
            base.OnAwake();
            World.RegisterOneFrame<DiceClickedEvent>();
        }

        protected override void Process(Entity entity, ref Dice first, ref DiceClickedEvent second, ref DiceCanSelectTag third, in float deltaTime)
        {
            entity.RemoveComponent<DiceCanSelectTag>();
            entity.AddComponent<MoveToHandTag>();
        }
    }
}