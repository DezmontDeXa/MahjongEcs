using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using Scellecs.Morpeh.Helpers;
using static Algorithms;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Constructor/" + nameof(SelectDiceSystem))]
	public sealed class SelectDiceSystem : UpdateSystem
	{
        private Entity _selectedEntity;

        public override void OnAwake()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            ref var grid = ref World.Filter.With<Grid>().First().GetComponent<Grid>();

            var worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var entity = Algorithms.GetNearestEntity(worldMousePosition);

            if (entity == null)
            {
                if (_selectedEntity != null)
                    HideDice(_selectedEntity);
                _selectedEntity = null;
                return;
            }

            ShowDice(entity);

            if (_selectedEntity != entity)
            {
                if (_selectedEntity != null)
                    HideDice(_selectedEntity);

                _selectedEntity = entity;
            }

            //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(grid.StartPoint, Input.mousePosition, Camera.main, out var localPoint))
            //{
            //    // Reverse Y for as grid system
            //    localPoint = new Vector2(localPoint.x, -localPoint.y);
            //}
            //ShowHideByHover();
        }

        private Entity GetAvailableEntity()
        {
            var worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var colliders = Algorithms.GetCollidersUnderPoint(worldMousePosition);
            var collidersToEntities = new Dictionary<Collider2D, Entity>();

            //TODO: оптимизировать.
            //Искать по тегу CanSelectedOptimizationTag,
            //который назаначается другой системой для дайса с эвентом Hover и его соседей
            foreach (var collider in colliders)
                if (collider.TryGetComponent<EntityRefMono>(out var refMono))
                    collidersToEntities.Add(collider, refMono.Entity);

            if (collidersToEntities.Count != 0)
            {
                var allPositions = World.Filter.With<GridPositionsList>().First().GetComponent<GridPositionsList>().ConstructorPlacedPositions;

                var avaliableEntities = new List<Entity>();
                foreach (var ent in collidersToEntities.Values)
                {
                    var neighbor = new Neighbor(ent.GetComponent<InGridPosition>().Position);
                    if (!neighbor.GetList(false).Any(
                        n =>
                        {
                            if (allPositions.Any(x => n.Equals(x.Value.Position)))
                                return true;
                            return false;
                        }))
                    {
                        avaliableEntities.Add(ent);
                    }
                }

                var entity = Algorithms.GetNearestEntity(
                    worldMousePosition,
                    avaliableEntities.Select(x => collidersToEntities.First(c=>c.Value == x).Key));

                return entity;
            }

            return null;
        }

        private void HideDice(Entity entity)
        {
            if (entity.IsNullOrDisposed()) return;
            if (entity.Has<PlacedConstructorDice>()) return;
            if (!entity.Has<SelectedConstructorDice>()) return;
            entity.RemoveComponent<SelectedConstructorDice>();
        }

        private void ShowDice(Entity entity)
        {
            if (entity.IsNullOrDisposed()) return;
            if (entity.Has<PlacedConstructorDice>()) return;
            entity.AddOrGet<SelectedConstructorDice>();
        }

        //private void ShowHideByHover()
        //{
        //    foreach (var entity in World.Filter.With<ConstructorDice>().Without<PlacedConstructorDice>().With<DiceImageRef>().With<DicePointerEnterEvent>())
        //    {
        //        ref var imgRef = ref entity.GetComponent<DiceImageRef>();
        //        imgRef.BackImage.color = _visibleColor;
        //    }

        //    foreach (var entity in World.Filter.With<ConstructorDice>().Without<PlacedConstructorDice>().With<DiceImageRef>().With<DicePointerExitEvent>())
        //    {
        //        ref var imgRef = ref entity.GetComponent<DiceImageRef>();
        //        imgRef.BackImage.color = _invisibleColor;
        //    }
        //}
    }


}