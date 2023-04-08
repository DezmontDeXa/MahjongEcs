using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(AssemblyThreeDicesSystem))]
	public sealed class AssemblyThreeDicesSystem : UpdateSystem 
	{
        private IamHand _hand;
        private Filter _filter;

        public override void OnAwake() 
		{
            _hand = World.Filter.With<IamHand>().First().GetComponent<IamHand>();
			_filter = World.Filter.With<Dice>().With<InHandTag>().Without<AssemblingTag>().Without<InAnimationTag>();
        }
	
		public override void OnUpdate(float deltaTime) 
		{
			var entities = _filter.ToList();
			var groups = entities.GroupBy(x => x.GetComponent<Dice>().Data.Id);
			var group3 = groups.FirstOrDefault(x => x.Count() >= 3);
			if (group3 == null) return;

			var assembly = group3.Take(3);

			foreach (var entity in assembly)
			{
				entity.RemoveComponent<InHandTag>();
				entity.RemoveComponent<InHandPosition>();
                entity.AddComponent<AssemblingTag>();
            }
        }
	}
}