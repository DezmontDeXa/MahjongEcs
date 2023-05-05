using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;
using UnityEngine;
using System;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public abstract class AttackSystem<Tconfig> : AttackSystem where Tconfig : EnemyConfig
    {
        [SerializeField] private GameSettings _gameSettings;
        protected Tconfig Config;
        private int _stepsToAttack;

        public override sealed void OnAwake()
        {
            AfterAwake();
        }

        public virtual void AfterAwake()
        {

        }

        public override sealed void OnUpdate(float deltaTime)
        {
            if (!Enabled) return;

            Config = _gameSettings.Difficulty.GetGirlConfigByType<Tconfig>();

            if (World.Filter.With<StartGameEvent>().Any())
                _stepsToAttack = 0;

            AfterUpdate(deltaTime);

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

        public virtual void AfterUpdate(float deltaTime)
        {

        }

        protected virtual void Attack()
        {

        }
    }

    public abstract class AttackSystem : UpdateSystem
    {
        public bool Enabled = false;
    }
}