using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Grid/" + nameof(GridClearSystem))]
	public sealed class GridClearSystem : SimpleUpdateSystem<ClearGridEvent>
	{
        protected override void Process(Entity entity, ref ClearGridEvent component, in float deltaTime)
        {
			foreach (var diceEntity in World.Filter.With<InGridPosition>())
			{
				ref var go = ref diceEntity.GetComponent<GameObjectRef>();
				Destroy(go.GameObject);
				World.RemoveEntity(diceEntity);
			}
        }
    }
}