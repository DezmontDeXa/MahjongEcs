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
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Orc/" + nameof(OrcAttackSystem))]
	public sealed class OrcAttackSystem : AttackSystem<OrcConfig>
	{
        protected override void Attack()
        {
            base.Attack();

            var dices = World.Filter.With<InGridPosition>().ToArray();
            var positions = dices.Select(x => x.GetComponent<InGridPosition>().Position).ToArray();

            Shuffle(dices);

            for (int i = 0; i < dices.Length; i++)
            {
                var dice = dices[i];
                var pos = positions[i];

                ref var p = ref dice.GetComponent<InGridPosition>();
                p.Position = pos;
            }
            World.CreateEntity().AddComponent<CanSelectChangedEvent>();
        }

        private static void Shuffle<T>(T[] arr)
        {
            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = Random.Range(0, i + 1);

                T tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }
    }
}