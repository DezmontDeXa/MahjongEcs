using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class IamHandMono : MonoProvider<IamHand> 
	{

        protected override void Initialize()
        {
            base.Initialize();
            GetData().Transform = (RectTransform)transform;
        }
    }
}