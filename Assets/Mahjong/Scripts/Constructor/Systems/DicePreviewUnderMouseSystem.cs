using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System;
using System.Threading;
using Unity.Burst.Intrinsics;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Constructor/" + nameof(DicePreviewUnderMouseSystem))]
	public sealed class DicePreviewUnderMouseSystem : SimpleUpdateSystem<DicePreviewUnderMouseSettings>
	{
		Vector3 _prevDicePosition;
        private InGridPositionMono _dice;

        protected override void Process(Entity entity, ref DicePreviewUnderMouseSettings settings, in float deltaTime)
        {
            var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			var _gridRect = settings.GridTransform.rect;

            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(settings.GridTransform, mousePos, Camera.main, out var inRectPoint))
            {
                inRectPoint = new Vector2(Mathf.Abs(inRectPoint.x + _gridRect.width / 2), Mathf.Abs(inRectPoint.y - _gridRect.height / 2));
                inRectPoint = new Vector2(
                    inRectPoint.x - settings.GridStartPoint.anchoredPosition.x, 
                    inRectPoint.y - Mathf.Abs(settings.GridStartPoint.anchoredPosition.y));

                UpdatePreviewDicePosition(settings, inRectPoint);
            }

            return;
        }

        private void UpdatePreviewDicePosition(DicePreviewUnderMouseSettings settings, Vector2 inRectPoint)
        {
            ref var grid = ref settings.GridTransform.GetComponent<GridMono>().GetData();

            float x = inRectPoint.x / Mathf.Abs(grid.Step.x);
            float y = inRectPoint.y / MathF.Abs(grid.Step.y);

            x = Algorithms.RoundToFraction(x, 0.5f);
            y = Algorithms.RoundToFraction(y, 0.5f);

            if(x<0 || y <0)
            {
                if(_dice != null)
                    Destroy(_dice.gameObject);
            }
            else
            {
                if (_dice == null)
                    _dice = Instantiate(settings.DicePrefab, settings.GridStartPoint);

                ref var data = ref _dice.GetData();
                data.Position = new Vector3(x, y);
            }

        }
    }
}