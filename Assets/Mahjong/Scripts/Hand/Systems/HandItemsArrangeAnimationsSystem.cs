using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using DG.Tweening;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HandItemsArrangeAnimationsSystem))]
	public sealed class HandItemsArrangeAnimationsSystem : SimpleUpdateSystem <Dice, InHandPosition>
    {
        [SerializeField] private float MoveAnimationDuration = 0.22f;
        private IamHand _hand;

        public override void OnUpdate(float deltaTime)
        {
            _hand = World.Filter.With<IamHand>().First().GetComponent<IamHand>();
            base.OnUpdate(deltaTime);
        }

        protected override void Process(Entity entity, ref Dice dice, ref InHandPosition pos, in float deltaTime)
        {
            if (entity.Has<InAnimationTag>()) return;

            if(pos.Position != pos.PrevPosition)
            {
                entity.AddComponent<InAnimationTag>();

                var _dice = dice;
                var _entity = entity;
                var _pos = pos;

                dice.Transform.SetParent(_hand.CellsTransforms[_pos.Position].parent);
                ((RectTransform)dice.Transform)
                    .DOAnchorPos(_hand.CellsTransforms[_pos.Position].anchoredPosition, MoveAnimationDuration)
                    .OnComplete(() =>
                    {
                        _dice.Transform.SetParent(_hand.CellsTransforms[_pos.Position]);
                        var rectTransform = (RectTransform)_dice.Transform;
                        rectTransform.anchoredPosition = new Vector2();
                        _entity.RemoveComponent<InAnimationTag>();
                    });

                pos.PrevPosition = pos.Position;
            }
        }
    }
}