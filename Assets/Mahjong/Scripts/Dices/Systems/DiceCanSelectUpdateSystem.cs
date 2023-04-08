using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using System.Collections.Generic;
using static Algorithms;
using System.Linq;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DiceCanSelectUpdateSystem))]
    public sealed class DiceCanSelectUpdateSystem : SimpleUpdateSystem<CanSelectChangedEvent>
    {
        [SerializeField] private Color _canSelectColor;
        [SerializeField] private Color _canNotSelectColor;

        protected override void Process(Entity entity, ref CanSelectChangedEvent @event, in float deltaTime)
        {
            var otherPoses = new Dictionary<Entity, Vector3>();
            foreach (var ent in World.Filter.With<InGridPosition>().With<DiceImageRef>())
                otherPoses.Add(ent, ent.GetComponent<InGridPosition>().Position);
            
            if (@event.Position != null)
                ProcessOneDice(@event, otherPoses);
            else
                ProcessAllDices(otherPoses);

            entity.RemoveComponent<CanSelectChangedEvent>();
        }

        private void ProcessAllDices(Dictionary<Entity, Vector3> otherPoses)
        {
            foreach (var ent in World.Filter.With<InGridPosition>().With<DiceImageRef>())
            {
                ref var pos = ref ent.GetComponent<InGridPosition>();
                ref var imgRef = ref ent.GetComponent<DiceImageRef>();

                if (Algorithms.PositionCanSelect(pos.Position, otherPoses.Values))
                    SetCanSelect(ent, imgRef);
                else
                    SetCanNotSelect(ent, imgRef);
            }
        }

        private void ProcessOneDice(CanSelectChangedEvent @event, Dictionary<Entity, Vector3> otherPoses)
        {
            var neighbor = new Neighbor(@event.Position.Value);
            var neighborDown = neighbor.Down;

            var nearExists = neighbor.SelectExists(otherPoses.Values, 2);
            nearExists.AddRange(neighborDown.SelectExists(otherPoses.Values, 1));

            foreach (var near in nearExists)
            {
                var pair = otherPoses.FirstOrDefault(p => p.Value.x == near.x && p.Value.y == near.y && p.Value.z == near.z);
                var nearEntity = pair.Key;

                ref var pos = ref nearEntity.GetComponent<InGridPosition>();
                ref var imgRef = ref nearEntity.GetComponent<DiceImageRef>();

                if (Algorithms.PositionCanSelect(pos.Position, otherPoses.Values))
                    SetCanSelect(nearEntity, imgRef);
                else
                    SetCanNotSelect(nearEntity, imgRef);
            }
        }

        private void SetCanSelect(Entity entity, DiceImageRef imgRef)
        {
            entity.AddOrGet<DiceCanSelectTag>();
            imgRef.BackImage.color = _canSelectColor;
            imgRef.Image.color = _canSelectColor;
        }

        private void SetCanNotSelect(Entity entity, DiceImageRef imgRef)
        {
            if (entity.Has<DiceCanSelectTag>())
                entity.RemoveComponent<DiceCanSelectTag>();

            imgRef.BackImage.color = _canNotSelectColor;
            imgRef.Image.color = _canNotSelectColor;
        }

    }
}