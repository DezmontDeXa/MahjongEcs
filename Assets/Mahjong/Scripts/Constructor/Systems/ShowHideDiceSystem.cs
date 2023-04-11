using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh.Experimental;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Constructor/" + nameof(ShowHideDiceSystem))]
	public sealed class ShowHideDiceSystem : SimpleUpdateSystem<DiceImageRef>
    {
        [SerializeField] private Color _invisibleColor = new Color(0, 0, 0, 0);
        [SerializeField] private Color _visibleColor = new Color(1, 1, 1, 1);

        protected override void Process(Entity entity, ref DiceImageRef imgRef, in float deltaTime)
        {
            if (entity.Has<SelectedConstructorDice>() || entity.Has<PlacedConstructorDice>())
                ShowDice(imgRef);
            else
                HideDice(imgRef);
        }

        private void ShowDice(DiceImageRef imgRef)
        {
            imgRef.BackImage.color = _visibleColor;
        }

        private void HideDice(DiceImageRef imgRef)
        {
            imgRef.BackImage.color = _invisibleColor;
        }
    }
}