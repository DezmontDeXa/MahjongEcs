using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;
using System.Collections.Generic;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Angel/" + nameof(AngelAttackSystem))]
	public sealed class AngelAttackSystem : AttackSystem<AngelConfig>
    {
		private List<KeyValuePair<DiceData, Vector3>> _dataAndPos = new List<KeyValuePair<DiceData, Vector3>>();

        public override void OnAwake() 
		{
        }
	
		public override void OnUpdate(float deltaTime) 
		{
			base.OnUpdate(deltaTime);

			foreach (var item in World.Filter.With<DiceTakedEvent>())
			{
				ref var diceTakedEvent = ref item.GetComponent<DiceTakedEvent>();
                var data = diceTakedEvent.Dice.Data;
                var pos = diceTakedEvent.Position;
				_dataAndPos.Add(new KeyValuePair<DiceData, Vector3>(data, pos));
				if (_dataAndPos.Count > 10)
					_dataAndPos.RemoveAt(0);
			}			
        }

        protected override void Attack()
        {
			ref var grid = ref World.Filter.With<Grid>().First().GetComponent<Grid>();

			for (int i = 0; i < Config.RestoreDicesCount; i++)
			{
				var posId = Random.Range(0, _dataAndPos.Count);
                var dataAndPos = _dataAndPos[posId];
				_dataAndPos.RemoveAt(posId);
				ReviveDice(grid, dataAndPos.Key, dataAndPos.Value);
            }
        }

        private void ReviveDice(Grid grid, DiceData data, Vector3 position)
        {
            var dice = Instantiate(Config.DicePrefab, grid.StartPoint);

			dice.Entity.AddComponent<DiceCanSelectTag>();
            var pos = dice.GetComponent<InGridPositionMono>();
            ref var posComp = ref pos.GetData();
            posComp.Position = position;
			dice.GetComponent<DiceMono>().GetData().Data = data;

            ref var @event = ref World.CreateEntity().AddComponent<CanSelectChangedEvent>();
			@event.Position = position;
        }
    }
}