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
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DiceCanSelectViewSystem))]
	public sealed class DiceCanSelectViewSystem : SimpleUpdateSystem<DiceImageRef, InGridPosition>
    {
        [SerializeField] private Color _canSelectColor;
        [SerializeField] private Color _canNotSelectColor;


        protected override void Process(Entity entity, ref DiceImageRef first, ref InGridPosition second, in float deltaTime)
        {
            if (entity.Has<DiceCanSelectTag>())
            {
                first.BackImage.color = _canSelectColor;
                first.Image.color = _canSelectColor;
            }
            else
            {
                first.BackImage.color = _canNotSelectColor;
                first.Image.color = _canNotSelectColor;
            }
        }
    }
}