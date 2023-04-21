using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Experimental;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Druid/" + nameof(DruidRootsViewSystem))]
	public sealed class DruidRootsViewSystem : SimpleUpdateSystem<HandCell, GameObjectRef> 
	{
		[SerializeField] private DruidConfig _druidConfig;

        protected override void Process(Entity entity, ref HandCell cell, ref GameObjectRef goRef, in float deltaTime)
        {
            if (entity.Has<RootedTag>())
            {
                if (entity.Has<RootsRef>()) return;
                ref var rootsRef = ref entity.AddComponent<RootsRef>();
                rootsRef.Roots = Instantiate(_druidConfig.RootEffectPrefab, goRef.GameObject.transform);
            }
            else
            {
                if (!entity.Has<RootsRef>()) return;
                ref var rootsRef = ref entity.GetComponent<RootsRef>();
                Destroy(rootsRef.Roots);
                entity.RemoveComponent<RootsRef>();
            }
        }
    }
}