using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class HandCellMono : MonoProvider<HandCell> 
	{
        protected override void Initialize()
        {
            base.Initialize();
            base.GetData().GameObject = gameObject;
        }
    }
}