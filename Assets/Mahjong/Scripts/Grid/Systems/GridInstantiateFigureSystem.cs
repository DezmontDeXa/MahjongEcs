using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Grid/" + nameof(GridInstantiateFigureSystem))]
	public sealed class GridInstantiateFigureSystem : SimpleUpdateSystem<InstantiateFigureEvent>
	{
        [SerializeField] private bool _sendDiceClickedEvent = true;

        protected override void Process(Entity entity, ref InstantiateFigureEvent @event, in float deltaTime)
        {
            foreach (var point in @event.Figure.Points)
            {
                var dice = Instantiate(@event.DicePrefab, @event.DicesContainer);
                var gridPos = dice.GetComponent<InGridPositionMono>();
                gridPos.GetData().Position = point;
                if(_sendDiceClickedEvent)
                    gridPos.Entity.AddComponent<DiceClickedEvent>();

            }
            World.CreateEntity().AddComponent<CanSelectChangedEvent>(); 
            World.CreateEntity().AddComponent<FigureGeneratedEvent>();
        }
    }
}