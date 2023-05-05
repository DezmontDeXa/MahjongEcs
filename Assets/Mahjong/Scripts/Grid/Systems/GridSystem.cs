using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using DG.Tweening;
using Scellecs.Morpeh.Helpers;
using Unity.VisualScripting;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Grid/" + nameof(GridSystem))]
	public sealed class GridSystem : GridUpdateSystem
    {
        public override void OnAwake()
        {
            base.OnAwake();
			DOTween.SetTweensCapacity(1250, 50);
        }

        protected override void Process(Entity gridEntity, Entity diceEntity, Grid grid, Dice dice, InGridPosition pos, InGridSize size)
        {
			var point = grid.StartPoint.transform.position;

			// Position on level
			point.x = pos.Position.x * grid.Step.x;
            point.y = pos.Position.y * grid.Step.y;

			// Offset by level
			point.x += pos.Position.z * grid.LevelOffset.x;
            point.y += pos.Position.z * grid.LevelOffset.y;

			// Move dice
            dice.Transform.SetParent( grid.StartPoint);
            var diceRectTransform = (RectTransform)dice.Transform;


            if (diceRectTransform != null && !diceRectTransform.gameObject.IsDestroyed())
            {
                if(diceRectTransform.anchoredPosition.x != point.x || diceRectTransform.anchoredPosition.y != point.y)
                    diceRectTransform.DOAnchorPos(new Vector2(point.x, point.y), 0.22f).SetId(diceRectTransform.gameObject);

                if (diceRectTransform.rect.width != size.Size.x || diceRectTransform.rect.height != size.Size.y)
                    diceRectTransform.DOSizeDelta(size.Size, 0.22f);
            }

            // Calculate sorting order
            pos.Canvas.sortingOrder = (int)(pos.Position.z * 1000) + (int)(Mathf.Abs(point.x)*2) + (int)(Mathf.Abs(point.y) * 2);

			// Show in next frame
			diceEntity.AddOrGet<ShowAfterMoveTag>();
        }
    }
}