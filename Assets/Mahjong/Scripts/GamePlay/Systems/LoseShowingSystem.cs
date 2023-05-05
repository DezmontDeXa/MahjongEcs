using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

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
            var panel = World.Filter.With<LosePanelTag>().With<GameObjectRef>().First().GetComponent<GameObjectRef>();
            panel.GameObject.SetActive(true);
        }
    }
}