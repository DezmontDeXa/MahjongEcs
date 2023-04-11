using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Constructor/" + nameof(ConstructorClickDiceSystem))]
	public sealed class ConstructorClickDiceSystem : UpdateSystem 
	{
		public override void OnAwake() 
		{
		}
	
		public override void OnUpdate(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Input.mousePosition;
                var worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                var entity = Algorithms.GetNearestEntity(worldMousePosition);
                if (entity == null) return;

                if (entity.Has<ConstructorDice>())
                    entity.AddComponent<DiceClickedEvent>();
            }
        }
	}
}