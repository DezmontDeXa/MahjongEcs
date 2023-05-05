using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Succub/" + nameof(SuccubBlockSystem))]
	public sealed class SuccubBlockSystem : UpdateSystem
    {
        [SerializeField] private GameSettings _gameSettings;
        private SuccubConfig _succubConfig;

        public override void OnAwake() 
		{
		}
	
		public override void OnUpdate(float deltaTime)
        {
            _succubConfig = _gameSettings.Difficulty.GetGirlConfigByType<SuccubConfig>();
            InstantiateEffectOnNewBlockedDices();
            RemoveEffectOnReleasedDices();
            UpdateBlock();
        }

        private void UpdateBlock()
        {
            foreach (var entity in World.Filter.With<Dice>().With<SuccubBlockedTag>().With<DiceCanSelectTag>())
                if(entity.Has<SuccubBlockedTag>())
                    entity.RemoveComponent<DiceCanSelectTag>();
        }

        private void InstantiateEffectOnNewBlockedDices()
        {
            foreach (var entity in World.Filter.With<SuccubBlock>().Without<SuccubBlockedTag>())
            {
                var go = entity.GetComponent<GameObjectRef>().GameObject;
                var effectGo = Instantiate(_succubConfig.SuccubDiceEffectPrefab, go.transform);
                ref var tag = ref entity.AddComponent<SuccubBlockedTag>();
                entity.RemoveComponent<DiceCanSelectTag>();
                tag.EffectGo = effectGo;
            }
        }

        private void RemoveEffectOnReleasedDices()
        {
            foreach (var entity in World.Filter.With<SuccubBlockedTag>().Without<SuccubBlock>())
            {
                ref var tag = ref entity.GetComponent<SuccubBlockedTag>();
                tag.EffectGo.GetComponent<Animator>().SetTrigger("Hide");
                entity.AddOrGet<DiceCanSelectTag>();
                entity.RemoveComponent<SuccubBlockedTag>();
            }
        }
    }
}