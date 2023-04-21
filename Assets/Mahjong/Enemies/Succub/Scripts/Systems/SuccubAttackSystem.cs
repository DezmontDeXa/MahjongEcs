using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;
using Scellecs.Morpeh.Helpers;

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

            var allInGridDiceEntities = World.Filter.With<InGridPosition>().Without<SuccubBlock>().ToList();

            if (allInGridDiceEntities.Count <= Config.CountOfBlockingPerAttack)
                return;

            for (int i = 0; i < Config.CountOfBlockingPerAttack; i++)
            {
                var randomEntity = allInGridDiceEntities[Random.Range(0, allInGridDiceEntities.Count)];
                randomEntity.AddComponent<SuccubBlock>();
            }
        }
    }
}