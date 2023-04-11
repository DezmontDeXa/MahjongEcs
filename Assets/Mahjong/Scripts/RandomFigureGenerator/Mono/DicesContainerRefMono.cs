using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [RequireComponent(typeof(RectTransform))]
	public sealed class DicesContainerRefMono : MonoProvider<DicesContainerRef> 
	{
        protected override void Initialize()
        {
            base.Initialize();
            GetData().Container = (RectTransform)transform;
        }
    }
}