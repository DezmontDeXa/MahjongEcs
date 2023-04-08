using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(RemoveAssembledDicesSystem))]
	public sealed class RemoveAssembledDicesSystem : SimpleUpdateSystem<Dice, AssembledTag>
	{
        protected override void Process(Entity entity, ref Dice dice, ref AssembledTag second, in float deltaTime)
        {
			Destroy(dice.Transform.gameObject);
			World.RemoveEntity(entity);
        }
    }
}