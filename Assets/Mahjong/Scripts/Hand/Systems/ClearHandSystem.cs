using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;
using System;
using DG.Tweening;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Hand/" + nameof(ClearHandSystem))]
	public sealed class ClearHandSystem : UpdateSystem 
	{
		public override void OnAwake() 
		{
		}
	
		public override void OnUpdate(float deltaTime) 
		{
			if(World.Filter.With<LoseEvent>().Any())
                ClearHand();
            if (World.Filter.With<WinEvent>().Any())
                ClearHand();
			        }

        private void ClearHand()
        {
			foreach (var entity in World.Filter.With<InHandPosition>())
            {
                ref var goRef = ref entity.GetComponent<InHandPosition>();
                Destroy(goRef.GameObject);
                World.RemoveEntity(entity);
            }
        }
    }
}