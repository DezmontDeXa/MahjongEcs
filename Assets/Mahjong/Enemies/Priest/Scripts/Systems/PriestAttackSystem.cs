using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Linq;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Priest/" + nameof(PriestAttackSystem))]
	public sealed class PriestAttackSystem : AttackSystem<PriestConfig> 
	{
        protected override void Attack()
        {
            base.Attack();
			var allEmptyDices = World.Filter.With<Dice>().With<InGridPosition>().Without<HolyWaterTag>().ToList();
			var selectedDice = allEmptyDices[Random.Range(0, allEmptyDices.Count)];

            var go = selectedDice.GetComponent<GameObjectRef>().GameObject;
            var effectGo = Instantiate(Config.PriestDiceEffectPrefab, go.transform);
            ref var tag = ref selectedDice.AddComponent<HolyWaterTag>();
            tag.EffectGo = effectGo;
        }

        public override void AfterUpdate(float deltaTime)
        {

            foreach (var entity in World.Filter.With<Dice>().With<InHandPosition>().With<HolyWaterTag>())
            {
                ref var tag = ref entity.GetComponent<HolyWaterTag>();
                tag.EffectGo.GetComponent<Animator>().SetTrigger("Hide");
                entity.RemoveComponent<HolyWaterTag>();
            }
        }
    }
}