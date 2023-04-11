using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using static Algorithms;
using System.Linq;
using System.Collections.Generic;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/Constructor/" + nameof(ConstructorGridFillerSystem))]
    public sealed class ConstructorGridFillerSystem : UpdateSystem
    {
        [SerializeField] private ConstructorDiceMono _invisibleDicePrefab;
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(17, 9);
        public override void OnAwake()
        {
            FillFirstLayer();
        }

        public override void OnUpdate(float deltaTime)
        {
            ref var grid = ref World.Filter.With<Grid>().First().GetComponent<Grid>();
            ProcessAddedDice(grid);
            ProcessRemovedDice(grid);
        }

        private void ProcessRemovedDice(Grid grid)
        {
            foreach (var entity in World.Filter.With<ConstructorDice>().With<InGridPosition>().With<PlacedConstructorDice>().With<DiceClickedEvent>())
            {
                var pos = entity.GetComponent<InGridPosition>().Position;
                RemoveNextLevelForPosition(grid, pos);
                AddThatLevelForPosition(grid, pos);
            }
        }

        private void ProcessAddedDice(Grid grid)
        {
            foreach (var entity in World.Filter.With<ConstructorDice>().With<InGridPosition>().Without<PlacedConstructorDice>().With<DiceClickedEvent>())
            {
                var pos = entity.GetComponent<InGridPosition>().Position;
                AddNextLevelForPosition(grid, pos);
                RemoveThatLevelForPosition(grid, pos);
            }
        }

        private void AddNextLevelForPosition(Grid grid, Vector3 pos)
        {
            var allPositions = World.Filter.With<GridPositionsList>().First().GetComponent<GridPositionsList>().AllPositions;

            var neighbors = new Neighbor(pos).Up;
            foreach (var neighbor in neighbors.GetList(0.5f))
                if (neighbor.Position.x >= 0 && neighbor.Position.y >= 0)
                    if(!allPositions.Any(x=> neighbor.Equals(x.Value.Position)))
                        InstantiateInvisibleDice(grid, neighbor.Position);
        }

        private void AddThatLevelForPosition(Grid grid, Vector3 pos)
        {
            var allPositions = World.Filter.With<GridPositionsList>().First().GetComponent<GridPositionsList>().AllPositions;

            var neighbors = new Neighbor(pos);
            foreach (var neighbor in neighbors.GetList(0.5f, true))
                if (neighbor.Position.x >= 0 && neighbor.Position.y >= 0)
                    if (!allPositions.Any(x => neighbor.Equals(x.Value.Position)))
                        InstantiateInvisibleDice(grid, neighbor.Position);
        }

        private void RemoveNextLevelForPosition(Grid grid, Vector3 removedPos)
        {
            var removedNeighbor = new Neighbor(removedPos);
            var upRemovedNeighbors = removedNeighbor.Up.GetList(0.5f);
            var realNeighbors = removedNeighbor.GetList(1f, false);

            var allPositions = World.Filter
                .With<ConstructorDice>()
                .With<InGridPosition>()
                .With<GameObjectRef>()
                .Select(x => 
                new KeyValuePair<Entity, InGridPosition>(
                    x, 
                    x.GetComponent<InGridPosition>()));

            // Не удалять соседей сверху, если у них есть иная опора
            foreach (var realneighbor in realNeighbors)
            {
                if (realneighbor == null) continue;
                // существующая позиция
                if(allPositions.Any(x=> realneighbor.Equals(x.Value)))
                {
                    var upRealNeighbors = realneighbor.Up.GetList(0.5f);
                    foreach (var upRealNeighbor in upRealNeighbors)
                        upRemovedNeighbors.RemoveAll(x => x.Equals(upRealNeighbor.Position));
                }
            }
            
            foreach (var enitityPos in allPositions)
            {
                if (upRemovedNeighbors.Any(x => x.Equals(enitityPos.Value.Position)))
                {
                    ref var goRef = ref enitityPos.Key.GetComponent<GameObjectRef>();
                    Destroy(goRef.GameObject);
                    World.RemoveEntity(enitityPos.Key);
                }
            }

        }

        private void RemoveThatLevelForPosition(Grid grid, Vector3 addedPos)
        { 
            var addedNeighbor = new Neighbor(addedPos);
            var forRemoveNeighbors = addedNeighbor.GetList(0.5f, false);
            var allPositions = World.Filter.With<GridPositionsList>().First().GetComponent<GridPositionsList>().AllPositions;

            foreach (var enitityPos in allPositions)
            {
                if (enitityPos.Key.IsNullOrDisposed()) continue;

                if (forRemoveNeighbors.Where(x=>x != null).Any(x => x.Equals(enitityPos.Value.Position)))
                {
                    ref var goRef = ref enitityPos.Key.GetComponent<GameObjectRef>();
                    Destroy(goRef.GameObject);
                    World.RemoveEntity(enitityPos.Key);
                }
            }
        }

        private void FillFirstLayer()
        {
            ref var grid = ref World.Filter.With<Grid>().First().GetComponent<Grid>();

            for (float x = 0; x < _gridSize.x; x += 0.5f)
            {
                for (float y = 0; y < _gridSize.y; y += 0.5f)
                {
                    InstantiateInvisibleDice(grid, new Vector3(x, y, 0));
                }
            }
        }

        private void InstantiateInvisibleDice(Grid grid, Vector3 position)
        {
            var dice = Instantiate(_invisibleDicePrefab, grid.StartPoint);
            var pos = dice.GetComponent<InGridPositionMono>();
            ref var posComp = ref pos.GetData();
            posComp.Position = position;
        }
    }
}