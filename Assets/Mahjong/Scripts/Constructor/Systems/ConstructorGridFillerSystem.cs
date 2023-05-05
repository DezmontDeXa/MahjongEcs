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

            if (World.Filter.With<ClearGridEvent>().Any())
                FillFirstLayer();
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
            var entities = World.Filter.With<ConstructorDice>().With<InGridPosition>().Without<PlacedConstructorDice>().With<DiceClickedEvent>().ToList();
            if (entities.Count == 0) return;
            var entityToInGridPosition = entities.Select(x => new KeyValuePair<Entity, InGridPosition>(x, x.GetComponent<InGridPosition>()));   
            var layerGroups = entityToInGridPosition.GroupBy(x => x.Value.Position.z).OrderBy(x => x.Key);

            foreach (var group in layerGroups)
            {
                foreach (var pair in group)
                {
                    var pos = pair.Value.Position;
                    AddNextLevelForPosition(grid, pos);
                    RemoveThatLevelForPosition(grid, pos);
                }
            }
        }

        private void AddNextLevelForPosition(Grid grid, Vector3 pos)
        {
            var allPositions = World.Filter.With<GridPositionsList>().FirstOrDefault()?.GetComponent<GridPositionsList>().AllPositions;
            if (allPositions == null) return;

            var neighbors = new Neighbor(pos).Up;
            foreach (var neighbor in neighbors.GetList())
                if (neighbor.Position.x >= 0 && neighbor.Position.y >= 0)
                    if(!allPositions.Any(x=> neighbor.Equals(x.Value.Position)))
                        InstantiateInvisibleDice(grid, neighbor.Position);
        }

        private void AddThatLevelForPosition(Grid grid, Vector3 pos)
        {
            var allPositions = World.Filter.With<GridPositionsList>().First().GetComponent<GridPositionsList>().AllPositions;

            var neighbors = new Neighbor(pos);
            foreach (var neighbor in neighbors.GetList(true))
                if (neighbor.Position.x >= 0 && neighbor.Position.y >= 0)
                    if (!allPositions.Any(x => neighbor.Equals(x.Value.Position)))
                        InstantiateInvisibleDice(grid, neighbor.Position);
        }

        private void RemoveNextLevelForPosition(Grid grid, Vector3 removedPos)
        {
            var removedNeighbor = new Neighbor(removedPos);
            var upRemovedNeighbors = removedNeighbor.Up.GetList();
            var realNeighbors = removedNeighbor.GetList(false);

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
                    var upRealNeighbors = realneighbor.Up.GetList();
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
            var forRemoveNeighbors = addedNeighbor.GetList(false);
            Dictionary<Entity, InGridPosition> allPositions = GetAllPositions();

            foreach (var enitityPos in allPositions)
            {
                if (enitityPos.Key.IsNullOrDisposed()) continue;

                if (forRemoveNeighbors.Where(x => x != null).Any(x => x.Equals(enitityPos.Value.Position)))
                {
                    ref var goRef = ref enitityPos.Key.GetComponent<GameObjectRef>();
                    Destroy(goRef.GameObject);
                    World.RemoveEntity(enitityPos.Key);
                }
            }
        }

        private Dictionary<Entity, InGridPosition> GetAllPositions()
        {
            return World.Filter.With<GridPositionsList>().First().GetComponent<GridPositionsList>().AllPositions;
            
            var entities = World.Filter.With<InGridPosition>();
            var result = new Dictionary<Entity, InGridPosition>();

            foreach (var entity in entities)
            {
                result.Add(entity, entity.GetComponent<InGridPosition>());
            }

            return result;

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

            if (World.Filter.With<GridPositionsList>().Any())
            {
                var allPosDict = World.Filter.With<GridPositionsList>().First().GetComponent<GridPositionsList>().AllPositions;
                if (!allPosDict.Any(x => x.Value.Position.Equals(position)))
                    allPosDict.Add(dice.Entity, pos.GetData());
            }
        }
    }
}