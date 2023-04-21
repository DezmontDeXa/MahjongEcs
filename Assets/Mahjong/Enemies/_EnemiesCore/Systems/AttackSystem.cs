using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;
using UnityEngine;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public abstract class AttackSystem<Tconfig> : UpdateSystem where Tconfig : EnemyConfig
    {
        [SerializeField] protected Tconfig Config;
        private int _stepsToAttack;

        public override void OnAwake()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            if (World.Filter.With<StartGameEvent>().Any())
                _stepsToAttack = 0;

            foreach (var @event in World.Filter.With<DiceTakedEvent>())
            {
                _stepsToAttack++;

                if (_stepsToAttack >= Config.StepsToAttack)
                {
                    Attack();
                    _stepsToAttack = 0;
                }
            }
        }

        protected virtual void Attack()
        {

        }
    }
}