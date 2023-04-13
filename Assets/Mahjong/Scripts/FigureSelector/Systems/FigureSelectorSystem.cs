using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/FigureSelectors/" + nameof(FigureSelectorSystem))]
	public sealed class FigureSelectorSystem : UpdateSystem
    {
        [SerializeField] private GameObject _dicePrefab;
        private Figure[] _figures;

        public override void OnAwake()
        {
            _figures = Resources.LoadAll<Figure>("Figures/");
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var @event in World.Filter.With<StartGameEvent>())
            {
                Process(@event, ref @event.GetComponent<StartGameEvent>(), in deltaTime);
            }
        }

        private void Process(Entity entity, ref StartGameEvent component, in float deltaTime)
        {
            var figure = _figures[Random.Range(0, _figures.Length)];

            var dicecContainerRef = World.Filter.With<DicesContainerRef>().FirstOrDefault();
            if (dicecContainerRef == null)
            {
                Debug.LogError("DicesContainerRef not found");
                return;
            }
            ref var container = ref dicecContainerRef.GetComponent<DicesContainerRef>();
            
            World.CreateEntity().AddComponent<ClearGridEvent>();

            ref var @event = ref World.CreateEntity().AddComponent<InstantiateFigureEvent>();
            @event.Figure = figure;
            @event.DicesContainer = container.Container;
            @event.DicePrefab = _dicePrefab;
        }
    }
}