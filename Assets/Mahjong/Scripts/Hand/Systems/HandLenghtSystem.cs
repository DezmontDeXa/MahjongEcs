using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HandLenghtSystem))]
	public sealed class HandLenghtSystem : SimpleUpdateSystem<IamHand>
	{
        [SerializeField] private GameSettings _gameSettings;
        private HandSettings _settings;

        public override void OnUpdate(float deltaTime)
        {
            _settings = _gameSettings.Difficulty.HandSettings;
            base.OnUpdate(deltaTime);
        }

        protected override void Process(Entity entity, ref IamHand hand, in float deltaTime)
        {
            for (var i = 0; i < hand.CellsTransforms.Length; i++)
            {
                var cellTrans = hand.CellsTransforms[i];
                var value = i < _settings.Lenght;
                cellTrans.gameObject.SetActive(value);

            }
        }
    }
}