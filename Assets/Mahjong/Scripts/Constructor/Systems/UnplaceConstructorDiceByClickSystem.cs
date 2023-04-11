using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Constructor/" + nameof(UnplaceConstructorDiceByClickSystem))]
	public sealed class UnplaceConstructorDiceByClickSystem : SimpleUpdateSystem<DiceClickedEvent>
    {
        protected override void Process(Entity entity, ref DiceClickedEvent component, in float deltaTime)
        {
            if (entity.Has<PlacedConstructorDice>())
            {
                entity.RemoveComponent<PlacedConstructorDice>();
                entity.RemoveComponent<DiceClickedEvent>();
            }
        }
    }
}