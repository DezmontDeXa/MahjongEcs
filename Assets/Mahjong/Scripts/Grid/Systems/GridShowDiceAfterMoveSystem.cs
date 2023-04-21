using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GridShowDiceAfterMoveSystem))]
	public sealed class GridShowDiceAfterMoveSystem : SimpleUpdateSystem<ShowAfterMoveTag, DiceImageRef>
	{
        protected override void Process(Entity entity, ref ShowAfterMoveTag tag, ref DiceImageRef refs, in float deltaTime)
        {
            refs.Image.color = new Color(1, 1, 1, 1);
            refs.BackImage.color = new Color(1, 1, 1, 1);
            entity.RemoveComponent<ShowAfterMoveTag>();
        }
    }
}