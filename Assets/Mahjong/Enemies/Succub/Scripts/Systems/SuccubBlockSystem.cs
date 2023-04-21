using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Threading.Tasks;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Succub/" + nameof(SuccubBlockSystem))]
	public sealed class SuccubBlockSystem : UpdateSystem 
	{
		[SerializeField] private SuccubConfig Config;

		public override void OnAwake() 
		{
		}
	
		public override void OnUpdate(float deltaTime)
        {
            InstantiateEffectOnNewBlockedDices();
            RemoveEffectOnReleasedDices();
        }

        private void RemoveEffectOnReleasedDices()
        {
            foreach (var entity in World.Filter.With<SuccubBlockedTag>().Without<SuccubBlock>())
            {
                ref var tag = ref entity.GetComponent<SuccubBlockedTag>();
                tag.EffectGo.GetComponent<Animator>().SetTrigger("Hide");
                entity.RemoveComponent<SuccubBlockedTag>();
            }
        }

        private void InstantiateEffectOnNewBlockedDices()
        {
            foreach (var entity in World.Filter.With<SuccubBlock>().Without<SuccubBlockedTag>())
            {
                var go = entity.GetComponent<GameObjectRef>().GameObject;
                var effectGo = Instantiate(Config.SuccubDiceEffectPrefab, go.transform);
                ref var tag = ref entity.AddComponent<SuccubBlockedTag>();
                tag.EffectGo = effectGo;
            }
        }
    }
}