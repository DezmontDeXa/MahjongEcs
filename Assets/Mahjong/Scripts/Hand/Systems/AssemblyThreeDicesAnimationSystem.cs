using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using static UnityEditor.PlayerSettings;
using DG.Tweening;
using Unity.VisualScripting;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(AssemblyThreeDicesAnimationSystem))]
    public sealed class AssemblyThreeDicesAnimationSystem : SimpleUpdateSystem<Dice, AssemblingTag>
    {
        [SerializeField] private float MoveAnimationDuration = 0.22f;
        private IamHand _hand;
        public override void OnUpdate(float deltaTime)
        {
            _hand = World.Filter.With<IamHand>().First().GetComponent<IamHand>();
            base.OnUpdate(deltaTime);
        }


        protected override void Process(Entity entity, ref Dice dice, ref AssemblingTag second, in float deltaTime)
        {
            if (entity.Has<InAnimationTag>()) return;
            entity.AddComponent<InAnimationTag>();
            entity.RemoveComponent<AssemblingTag>();

            var _dice = dice;
            var _entity = entity;

            dice.Transform.SetParent(_hand.AssemblyPoint);

            var rectTrans = (RectTransform)dice.Transform;
            if (rectTrans == null || rectTrans.gameObject.IsDestroyed())
                return;
            rectTrans
            .DOAnchorPos(_hand.AssemblyPoint.anchoredPosition, MoveAnimationDuration)
            .OnComplete(() =>
            {
                _dice.Transform.SetParent(_hand.AssemblyPoint);
                var rectTransform = (RectTransform)_dice.Transform;
                rectTransform.anchoredPosition = new Vector2();
                _entity.RemoveComponent<InAnimationTag>();
                _entity.AddComponent<AssembledTag>();
            }).SetId(rectTrans.gameObject);

        }
    }
}