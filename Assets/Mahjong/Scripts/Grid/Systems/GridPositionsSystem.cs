using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;
using System.Collections.Generic;
using Scellecs.Morpeh.Helpers;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Grid/" + nameof(GridPositionsSystem))]
	public sealed class GridPositionsSystem : UpdateSystem 
	{
		public override void OnAwake() 
		{
		}
	
		public override void OnUpdate(float deltaTime)
        {
            var gridEntity = World.Filter.With<Grid>().First();
            ref GridPositionsList list = ref gridEntity.AddOrGet<GridPositionsList>();
                
            var allPositions = World.Filter
                .With<ConstructorDice>()
                .With<InGridPosition>()
                .With<GameObjectRef>()
                .Select(x =>
                new KeyValuePair<Entity, InGridPosition>(
                    x,
                    x.GetComponent<InGridPosition>()));
            list.AllPositions = new Dictionary<Entity, InGridPosition>();
            foreach (var pos in allPositions)
                list.AllPositions.Add(pos.Key, pos.Value);

            list.ConstructorPlacedPositions = new Dictionary<Entity, InGridPosition>();
            foreach (var entiti in World.Filter
                .With<ConstructorDice>()
                .With<InGridPosition>()
                .With<PlacedConstructorDice>()
                .Select(x =>
                new KeyValuePair<Entity, InGridPosition>(
                    x,
                    x.GetComponent<InGridPosition>())))
            {
                list.ConstructorPlacedPositions.Add(entiti.Key, entiti.Value);
            }

        }
	}
}