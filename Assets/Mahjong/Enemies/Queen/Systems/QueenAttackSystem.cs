using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Collections.Generic;
using System.Linq;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Queen/" + nameof(QueenAttackSystem))]
	public sealed class QueenAttackSystem : AttackSystem<QueenConfig> 
	{
		private List<StepData> _datas;

        public override void AfterAwake()
        {
            base.AfterAwake();
			_datas = new List<StepData>();
        }

        public override void AfterUpdate(float deltaTime)
        {
            base.AfterUpdate(deltaTime);
			foreach (var eventEntity in World.Filter.With<DiceTakedEvent>())
			{
				ref var takedEvent = ref eventEntity.GetComponent<DiceTakedEvent>();
				_datas.Add(new StepData(takedEvent.Entity, takedEvent.Dice, takedEvent.Position));
			}
        }

        protected override void Attack()
        {
            base.Attack();
            foreach (var diceEntity in World.Filter.With<InHandPosition>())
            {
                var stepData=  _datas.FirstOrDefault(x => x.Entity.ID == diceEntity.ID);
                if (stepData == null) continue;
                diceEntity.RemoveComponent<InHandPosition>();
                diceEntity.RemoveComponent<InHandTag>();
                ref var pos = ref diceEntity.AddComponent<InGridPosition>();
                pos.Position = stepData.Position;
                pos.Canvas = diceEntity.GetComponent<GameObjectRef>().GameObject.GetComponent<Canvas>();
            }

            World.CreateEntity().AddComponent<CanSelectChangedEvent>();

            _datas.Clear();
        }


        private class StepData
		{
            public StepData(Entity entity, Dice dice, Vector3 position)
            {
                Entity = entity;
                Dice = dice;
                Position = position;
            }

            public Entity Entity { get; }
			public Dice Dice { get; }
			public Vector3 Position { get; }
		}
    }



}