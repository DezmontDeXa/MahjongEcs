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
	[CreateAssetMenu(menuName = "ECS/Systems/Gameplay/" + nameof(WinCheckSystem))]
	public sealed class WinCheckSystem : SimpleUpdateSystem<ThreeDicesAssembledEvent>
	{
        protected override void Process(Entity entity, ref ThreeDicesAssembledEvent component, in float deltaTime)
        {
			if (!World.Filter.With<InGridPosition>().Any())
			{
				World.CreateEntity().AddComponent<WinEvent>();
				return;
			}

            var allDices = World.Filter.With<Dice>().Select(x => x.GetComponent<Dice>()).ToList();
            var groups = allDices.GroupBy(x => x.Data.Id);

            if (groups.All(x => x.Count() < 3))
            {
                World.CreateEntity().AddComponent<WinEvent>();
                return;
            }

        }
    }
}