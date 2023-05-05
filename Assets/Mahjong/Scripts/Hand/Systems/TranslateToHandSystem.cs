using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Linq;
using Scellecs.Morpeh.Helpers.OneFrame;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/Hand/" + nameof(TranslateToHandSystem))]
    public sealed class TranslateToHandSystem : SimpleUpdateSystem<Dice, InGridPosition, MoveToHandTag>
    {
       private IamHand _hand;

        public override void OnAwake()
        {
            base.OnAwake();
            World.RegisterOneFrame<MoveToHandTag>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            _hand = World.Filter.With<IamHand>().First().GetComponent<IamHand>();
        }

        protected override void Process(Entity entity, ref Dice dice, ref InGridPosition pos, ref MoveToHandTag tag, in float deltaTime)
        {
            ref var @event = ref World.CreateEntity().AddComponent<CanSelectChangedEvent>();
            @event.Position = pos.Position;
            var posVector = pos.Position;

            entity.GetComponent<InGridPosition>().Canvas.sortingOrder = 100500;

            entity.RemoveComponent<InGridPosition>();
            entity.RemoveComponent<MoveToHandTag>();
            entity.AddComponent<InHandTag>();



            ref var diceTakedEvent = ref World.CreateEntity().AddComponent<DiceTakedEvent>();
            diceTakedEvent.Entity = entity;
            diceTakedEvent.Dice = dice;
            diceTakedEvent.Position = posVector;
        }
    }
}