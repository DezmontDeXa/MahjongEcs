using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using DG.Tweening;
using Sirenix.OdinInspector.Editor.Validation;
using System.Linq;
using System.Collections.Generic;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HandItemsArrangeSystem))]
    public sealed class HandItemsArrangeSystem : UpdateSystem
    {
        private Filter _filter;
        private IamHand _hand;

        public override void OnAwake()
        {
            _filter = World.Filter.With<Dice>().With<InHandTag>();
        }

        public override void OnUpdate(float deltaTime)
        {
            _hand = World.Filter.With<IamHand>().First().GetComponent<IamHand>();

            // sorting all for keep exists places
            var all = World.Filter
                .With<Dice>().With<InHandTag>()
                .OrderBy(x =>
                { 
                if (x.Has<InHandPosition>())
                    return x.GetComponent<InHandPosition>().Position;
                else
                    return 1000;
                }).ToList();

            var _pos = 0;
            foreach (var entity in all)
            {
                if (entity.Has<InHandPosition>())
                {
                    ref var inHandPos = ref entity.GetComponent<InHandPosition>();
                    inHandPos.Position = _pos;
                }
                else
                {
                    ref var inHandPos = ref entity.AddOrGet<InHandPosition>();
                    inHandPos.Position = _pos;
                    inHandPos.PrevPosition = -1;
                }

                _pos++;
            }
        }
    }
}