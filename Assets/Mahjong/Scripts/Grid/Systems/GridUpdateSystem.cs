using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DiceCanSelectUpdateSystem))]
    public abstract class GridUpdateSystem : UpdateSystem
    {
        private Filter _grids;
        private Filter _dices;

        public override void OnAwake()
        {
            _grids = World.Filter.With<Grid>();
            _dices = World.Filter.With<Dice>().With<InGridPosition>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var gridEntity in _grids)
            {
                var grid = gridEntity.GetComponent<Grid>();
                foreach (var diceEntity in _dices)
                {
                    var dice = diceEntity.GetComponent<Dice>();
                    var pos = diceEntity.GetComponent<InGridPosition>();
                    var size = diceEntity.GetComponent<InGridSize>();

                    Process(gridEntity, diceEntity, grid, dice, pos, size);
                }
            }
        }

        protected abstract void Process(Entity gridEntity, Entity diceEntity, Grid grid, Dice dice, InGridPosition pos, InGridSize size);
    }
}