using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Linq;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Hand/" + nameof(HandFilledSystem))]
	public sealed class HandFilledSystem : SimpleUpdateSystem<InHandCountChangedEvent>
    {
        [SerializeField] private GameSettings _gameSettings;
        private HandSettings _settings;

        public override void OnUpdate(float deltaTime)
        {
            _settings = _gameSettings.Difficulty.HandSettings;
            base.OnUpdate(deltaTime);
        }

        protected override void Process(Entity entity, ref InHandCountChangedEvent component, in float deltaTime)
        {
			var count = World.Filter.With<InHandTag>().Count();
			if(count >= _settings.Lenght)
			{
				World.CreateEntity().AddComponent<LoseEvent>();
			}

			//entity.RemoveComponent<InHandCountChangedEvent>();
        }
    }
}