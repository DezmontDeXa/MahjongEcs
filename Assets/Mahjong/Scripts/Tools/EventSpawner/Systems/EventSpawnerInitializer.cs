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
	[CreateAssetMenu(menuName = "ECS/Systems/Tools/" + nameof(EventSpawnerInitializer))]
	public sealed class EventSpawnerInitializer : SimpleUpdateSystem<EventSpawnerInvoker> 
	{
        protected override void Process(Entity entity, ref EventSpawnerInvoker component, in float deltaTime)
        {
            component.World = World;
        }
    }
}