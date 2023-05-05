using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Linq;
using System;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Grid/" + nameof(GridClearSystem))]
	public sealed class GridClearSystem : UpdateSystem
	{
        public override void OnAwake()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
			if (World.Filter.With<ClearGridEvent>().Any())
				Clear();

            if (World.Filter.With<LoseEvent>().Any())
                Clear();

            if (World.Filter.With<InstantiateFigureEvent>().Any())
                Clear();
        }

        private void Clear()
        {
            foreach (var diceEntity in World.Filter.With<InGridPosition>())
            {
                ref var go = ref diceEntity.GetComponent<GameObjectRef>();
                Destroy(go.GameObject);
                World.RemoveEntity(diceEntity);
            }
        }
    }
}