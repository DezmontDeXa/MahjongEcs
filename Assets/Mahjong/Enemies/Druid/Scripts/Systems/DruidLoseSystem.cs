using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using System.Linq;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/Enemies/Druid/" + nameof(DruidLoseSystem))]
	public sealed class DruidLoseSystem : SimpleUpdateSystem<InHandCountChangedEvent>
    {
        [SerializeField] private GameSettings _gameSettings;
        private HandSettings _handSettings;

        public override void OnUpdate(float deltaTime)
        {
            _handSettings = _gameSettings.Difficulty.HandSettings;
            base.OnUpdate(deltaTime);
        }

        protected override void Process(Entity entity, ref InHandCountChangedEvent component, in float deltaTime)
        {
            //World.CreateEntity().AddComponent<LoseEvent>();

            var rootedCount = World.Filter.With<RootedTag>().Count();
            var count = World.Filter.With<InHandTag>().Count();
            if (count >= _handSettings.Lenght - rootedCount)
            {
                World.CreateEntity().AddComponent<LoseEvent>();
            }
        }
    }
}