using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Linq;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/Gameplay/" + nameof(WinShowingSystem))]
    public sealed class WinShowingSystem : SimpleUpdateSystem<WinEvent>
    {
        protected override void Process(Entity entity, ref WinEvent component, in float deltaTime)
        {
            var panel = World.Filter.With<WinPanelTag>().With<GameObjectRef>().First().GetComponent<GameObjectRef>();
            panel.GameObject.SetActive(true);
        }
    }
}