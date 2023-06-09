using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class InGridPositionMono : MonoProvider<InGridPosition> 
	{
        protected override void Initialize()
        {
            base.Initialize();
            GetData().Canvas = GetComponent<Canvas>();
        }
    }
}