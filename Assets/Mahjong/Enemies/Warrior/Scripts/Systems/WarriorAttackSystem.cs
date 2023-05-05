using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System;
using System.Linq;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/Enemies/Warrior/" + nameof(WarriorAttackSystem))]
    public sealed class WarriorAttackSystem : AttackSystem<WarriorConfig>
    {
        public override void AfterUpdate(float deltaTime)
        {
            if (World.Filter.With<LoseEvent>().Any() || World.Filter.With<WinEvent>().Any())
            {
                Reset();
            }


            if (World.Filter.With<StartGameEvent>().Any())
            {
                var timerViewEntity = World.Filter.With<WarriorTimerView>().FirstOrDefault();
                if (timerViewEntity == null) return;
                ref var timerView = ref timerViewEntity.GetComponent<WarriorTimerView>();
                timerView.TimerPanel.SetActive(true);

                ref var time = ref World.CreateEntity().AddComponent<WarriorTime>();
                time.LostTime = Config.StartTime;

                return;
            }

            if (!World.Filter.With<WarriorTime>().Any()) return;

            ref var timeComponent = ref World.Filter.With<WarriorTime>().First().GetComponent<WarriorTime>();
            timeComponent.LostTime -= deltaTime;

            if (World.Filter.With<DiceTakedEvent>().Any())
                timeComponent.LostTime += Config.TimeForDiceTaked;

            if (World.Filter.With<ThreeDicesAssembledEvent>().Any())
                timeComponent.LostTime += Config.TimeForDicesAssebled;

            if (timeComponent.LostTime > Config.StartTime)
                timeComponent.LostTime = Config.StartTime;

            if (timeComponent.LostTime < 0)
                timeComponent.LostTime = 0;

            if (timeComponent.LostTime == 0)
            {
                World.CreateEntity().AddComponent<LoseEvent>();
                Reset();
                return;
            }

            UpdateView();
        }

        private void Reset()
        {
            World.RemoveEntity(World.Filter.With<WarriorTime>().First());

            var timerViewEntity = World.Filter.With<WarriorTimerView>().First();
            ref var timerView = ref timerViewEntity.GetComponent<WarriorTimerView>();
            timerView.TimerPanel.SetActive(false);
        }

        private void UpdateView()
        {
            ref var timeComponent = ref World.Filter.With<WarriorTime>().First().GetComponent<WarriorTime>();

            var timerViewEntity = World.Filter.With<WarriorTimerView>().FirstOrDefault();
            if (timerViewEntity == null) return;
            ref var timerView = ref timerViewEntity.GetComponent<WarriorTimerView>();
            timerView.TimerText.text = new TimeSpan(0, 0, (int)timeComponent.LostTime).ToString("mm\\:ss");
            timerView.FillerImage.fillAmount = timeComponent.LostTime / Config.StartTime;
        }
    }
}