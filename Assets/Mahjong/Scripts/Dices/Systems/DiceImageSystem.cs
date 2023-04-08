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
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DiceImageSystem))]
	public sealed class DiceImageSystem : SimpleUpdateSystem<Dice, DiceImageRef> 
	{
        private Filter _filter;

        protected override void Process(Entity entity, ref Dice dice, ref DiceImageRef second, in float deltaTime)
        {
			second.Image.sprite = dice.Data.Sprite;
			//entity.RemoveComponent<DiceImageRef>();
        }
    }
}