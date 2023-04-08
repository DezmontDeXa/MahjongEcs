using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GridSystem))]
	public sealed class GridSystem : GridUpdateSystem
    {
        protected override void Process(Entity gridEntity, Entity diceEntity, Grid grid, Dice dice, InGridPosition pos)
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
			diceRectTransform.anchoredPosition = new Vector2(point.x, point.y);

			// Calculate sorting order
			pos.Canvas.sortingOrder = (int)(pos.Position.z * 1000) + (int)(Mathf.Abs(point.x)*2) + (int)(Mathf.Abs(point.y) * 2);
        }
    }
}