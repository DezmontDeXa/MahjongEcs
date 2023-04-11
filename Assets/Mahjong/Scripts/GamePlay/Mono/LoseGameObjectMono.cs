using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Events;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class LoseGameObjectMono : MonoProvider<LoseGameObject> 
	{
        public UnityEvent Show;
        protected override void Initialize()
        {
            base.Initialize();
        }

        private void Update()
        {
            ref var data = ref GetData();
            if (data.Shown)
            {
                OnShow();
                data.Shown = false;
            }
        }

        private void OnShow()
        {
            Show?.Invoke();
        }
    }
}