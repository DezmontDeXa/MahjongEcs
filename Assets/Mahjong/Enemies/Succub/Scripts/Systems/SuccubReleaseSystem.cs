using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Collections.Generic;
using System;
using static Algorithms;
using System.Linq;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/Enemies/Succub/" + nameof(SuccubReleaseSystem))]
    public sealed class SuccubReleaseSystem : UpdateSystem
    {
        public override void OnAwake()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var eventEntity in World.Filter.With<DiceTakedEvent>())
            {
                var allBlockedPositions = GetAllBlockedPositions();
                if (allBlockedPositions == null) return;
                ref var diceTakedEvent = ref eventEntity.GetComponent<DiceTakedEvent>();
                var pos = diceTakedEvent.Position;

                ReleaseNeighbors(pos, allBlockedPositions);
            }
        }

        private List<KeyValuePair<Entity, InGridPosition>> GetAllBlockedPositions()
        {
            var gridPositionsListEntity = World.Filter.With<GridPositionsList>().FirstOrDefault();
            if (gridPositionsListEntity == null) return null;
            ref var gridPositionListComponent = ref gridPositionsListEntity.GetComponent<GridPositionsList>();
            var allPositions = gridPositionListComponent.AllPositions;
            return allPositions.Where(x => x.Key.Has<SuccubBlockedTag>()).ToList();
        }

        private void ReleaseNeighbors(Vector3 pos, List<KeyValuePair<Entity, InGridPosition>> allBlockedPositions)
        {
            var neighbor = new Neighbor(pos, 0.5f);

            var forReleaseNeighbors = new List<Neighbor>
            {
                neighbor.Left.Left,
                neighbor.Left.Left.Top,
                neighbor.Left.Left.Bottom,

                neighbor.Right.Right,
                neighbor.Right.Right.Top,
                neighbor.Right.Right.Bottom,
            };


            foreach (var releaseNeighbor in forReleaseNeighbors)
            {
                foreach (var existPosPair in allBlockedPositions)
                {
                    if (releaseNeighbor.Equals(existPosPair.Value.Position))
                    {
                        existPosPair.Key.RemoveComponent<SuccubBlock>();
                    }
                }
            }
        }
    }
}