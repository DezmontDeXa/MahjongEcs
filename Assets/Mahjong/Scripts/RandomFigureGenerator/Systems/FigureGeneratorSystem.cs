using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System;
using UnityEngine.UIElements;
using System.Linq;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/FigureGenerator/" + nameof(FigureGeneratorSystem))]
	public sealed class FigureGeneratorSystem : SimpleUpdateSystem<RequestGenerateFigureEvent>
	{
		[SerializeField] private RandomFigureGeneratorSettings _settings;

        protected override void Process(Entity entity, ref RequestGenerateFigureEvent component, in float deltaTime)
        {
			ref var grid = ref World.Filter.With<Grid>().First().GetComponent<Grid>();

            var figure = GenerateFigure();

            World.CreateEntity().AddComponent<ClearGridEvent>();
            ref var evn = ref World.CreateEntity().AddComponent<InstantiateFigureEvent>();
            evn.Figure = figure;
            evn.DicesContainer = grid.StartPoint;
            evn.DicePrefab = _settings.DicePrefab;
        }


        private Figure GenerateFigure()
        {
            return new Figure()
            {
                Points = new System.Collections.Generic.List<Vector3>() 
                {
                    new Vector3(0,0,0),
                    new Vector3(1,1,0),
                    new Vector3(2,2,0),
                }
            };
        }

    }
}