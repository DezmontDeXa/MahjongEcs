using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;
using System;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/Enemies/Druid/" + nameof(DruidAttackSystem))]
    public sealed class DruidAttackSystem : AttackSystem
    {
        [SerializeField] private DruidConfig _druidConfig;

        protected override int GetAttackRate()
        {
            return _druidConfig.StepsToAttack;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            foreach (var @event in World.Filter.With<ThreeDicesAssembledEvent>())
                UnRoot();
        }

        private void UnRoot()
        {
            var entities = World.Filter.With<HandCell>().With<RootedTag>().ToList();
            if (entities.Count == 0) return;

            var lastEntity = entities.OrderBy(x => x.GetComponent<HandCell>().InHandIndex).First();
            lastEntity.RemoveComponent<RootedTag>();
        }

        protected override void Attack()
        {
            base.Attack();
            var entities = World.Filter.With<HandCell>().Without<RootedTag>().ToList();

            if (entities.Count == 0) return;

            var lastEntity = entities.OrderByDescending(x => x.GetComponent<HandCell>().InHandIndex).First();
            lastEntity.AddComponent<RootedTag>();
        }
    }
}