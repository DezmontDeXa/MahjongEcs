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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(FigureImageGeneratorSystem))]
    public sealed class FigureImageGeneratorSystem : SimpleUpdateSystem<FigureGeneratedEvent>
    {
        protected override void Process(Entity entity, ref FigureGeneratedEvent component, in float deltaTime)
        {
            var allDatas = Resources.LoadAll<DiceData>("DiceDatas");
            var groups = allDatas.GroupBy(x => x.Type).ToList();
            var selectedGroup = groups[Random.Range(0, groups.Count - 1)].ToList();

            var dicesEntities = World.Filter.With<DiceImageRef>().ToList();
            var count = dicesEntities.Count;

            int cnt = 0;
            var currentData = selectedGroup[Random.Range(0, selectedGroup.Count)];
            foreach (var diceEntity in dicesEntities)
            {
                diceEntity.GetComponent<Dice>().Data = currentData;
                cnt++;

                if (cnt >= 3)
                {
                    currentData = selectedGroup[Random.Range(0, selectedGroup.Count)];
                    cnt = 0;
                }
            }

            entity.RemoveComponent<FigureGeneratedEvent>();
        }
    }
}