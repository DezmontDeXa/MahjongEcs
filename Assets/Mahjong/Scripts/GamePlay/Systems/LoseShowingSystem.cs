using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Gameplay/" + nameof(LoseShowingSystem))]
	public sealed class LoseShowingSystem : SimpleUpdateSystem<LoseEvent>
	{
        protected override void Process(Entity entity, ref LoseEvent component, in float deltaTime)
        {
			ref var lose = ref World.Filter.With<LoseGameObject>().First().GetComponent<LoseGameObject>();
			lose.Shown = true;
            entity.RemoveComponent<LoseEvent>();
        }
    }
}