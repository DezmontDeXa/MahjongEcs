using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Linq;
using System.Collections.Generic;
using static Algorithms;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Constructor/" + nameof(RFG_ConstructorActivateSystem))]
	public sealed class RFG_ConstructorActivateSystem : SimpleUpdateSystem<FigureGeneratedEvent>
	{
        protected override void Process(Entity entity, ref FigureGeneratedEvent component, in float deltaTime)
        {
            var allPositions = World.Filter.With<GridPositionsList>().First().GetComponent<GridPositionsList>().AllPositions;

            foreach (var diceEntity in World.Filter.With<GeneratedDiceTag>())
			{
                var pos = diceEntity.GetComponent<InGridPosition>();
                var existEntity = allPositions.FirstOrDefault(p => Algorithms.CompareVector3(pos.Position, p.Value.Position)).Key;

                if (existEntity != null)
                {
                    existEntity.AddOrGet<DiceClickedEvent>();
                    var go = diceEntity.GetComponent<GameObjectRef>().GameObject;
                    Destroy(go);
                    World.RemoveEntity(diceEntity);
                }
                else
                {
                    diceEntity.AddComponent<DiceClickedEvent>();
                }
            }
        }
    }


}