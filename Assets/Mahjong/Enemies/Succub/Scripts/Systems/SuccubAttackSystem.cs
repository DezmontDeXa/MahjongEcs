using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;
using Scellecs.Morpeh.Helpers;
using System.Collections.Generic;
using static Algorithms;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/Enemies/Succub/" + nameof(SuccubAttackSystem))]
    public sealed class SuccubAttackSystem : AttackSystem<SuccubConfig>
    {
        protected override void Attack()
        {
            base.Attack();

            var allPositions = World.Filter.With<GridPositionsList>().Select(x => x.GetComponent<GridPositionsList>()).First().AllPositions;

            var allInGridDiceEntities = World.Filter.With<InGridPosition>().Without<SuccubBlock>().ToList();
            if (allInGridDiceEntities.Count <= Config.CountOfBlockingPerAttack)
                return;

            for (int i = 0; i < Config.CountOfBlockingPerAttack; i++)
            {
                var randomEntity = allInGridDiceEntities[Random.Range(0, allInGridDiceEntities.Count)];
                if (CanBlock(randomEntity, allPositions))
                    randomEntity.AddComponent<SuccubBlock>();
            }
        }

        private bool CanBlock(Entity randomEntity, Dictionary<Entity, InGridPosition> allPositions)
        {
            var position = randomEntity.GetComponent<InGridPosition>().Position;
            var exitsPositions = allPositions.Select(x => x.Value.Position).ToArray();
            var neighbor = new Neighbor(position);

            var leftChain = CheckSide(neighbor, allPositions, GetLefts);
            var rightChain = CheckSide(neighbor, allPositions, GetRights);

            if (leftChain.IsOpen && leftChain.Chain.Count > 0) 
                return true;
            if (rightChain.IsOpen && rightChain.Chain.Count > 0) 
                return true;


            return false;
        }

        /// TODO: —читать кол-ко шагов, что бы знать сколько было соседей. ≈сли соседей не было - false
        private CheckSideChain CheckSide(Neighbor neighbor, Dictionary<Entity, InGridPosition> allPositions, System.Func<Neighbor, List<Neighbor>> nextNeighbors, List<Entity> chain=null)
        {
            chain ??= new List<Entity>();

            var neighbors = nextNeighbors.Invoke(neighbor);

            bool isOpen = true;

            foreach (var n in neighbors)
            {
                foreach (var p in allPositions)
                {
                    if (n.Equals(p.Value.Position))
                    {
                        if (p.Key.Has<SuccubBlock>())
                        {
                            isOpen = false;
                            break;
                        }

                        isOpen = true;
                        chain.Add(p.Key);
                        return CheckSide(n, allPositions, nextNeighbors, chain);
                    }
                }
            }

            return new CheckSideChain(isOpen, chain);
        }

        private class CheckSideChain
        {
            public CheckSideChain(bool isOpen, List<Entity> chain)
            {
                IsOpen = isOpen;
                Chain = chain;
            }

            public bool IsOpen { get; }

            public List<Entity> Chain { get; }
        }

        private List<Neighbor> GetLefts(Neighbor neighbor)
        {
            return new List<Neighbor>
                {
                    neighbor.Left.Left,
                    neighbor.Left.Left.Top,
                    neighbor.Left.Left.Bottom,
                };

        }

        private List<Neighbor> GetRights(Neighbor neighbor)
        {
            return new List<Neighbor>
                {
                    neighbor.Right.Right,
                    neighbor.Right.Right.Top,
                    neighbor.Right.Right.Bottom,
                };
        }

    }
}