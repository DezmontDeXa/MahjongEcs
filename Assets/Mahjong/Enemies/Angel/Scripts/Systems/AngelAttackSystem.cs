using Scellecs.Morpeh.Systems;
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
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Angel/" + nameof(AngelAttackSystem))]
	public sealed class AngelAttackSystem : UpdateSystem 
	{
		[SerializeField] private AngelConfig _config;
        private int _stepsToAttack;

        public override void OnAwake() 
		{
        }
	
		public override void OnUpdate(float deltaTime) 
		{
			if (World.Filter.With<StartGameEvent>().Any())
				_stepsToAttack = 0;

			foreach (var item in World.Filter.With<DiceTakedEvent>())
				_stepsToAttack++;

			if (_stepsToAttack >= _config.StepsToAttack)
			{
				Attack();
				_stepsToAttack = 0;
            }
        }

        private void Attack()
        {

        }
    }
}