using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Providers;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GridMono : MonoProvider<Grid> 
	{        
        protected override void Initialize()
        {
            base.Initialize();
            GetData().Transform = transform;
        }
    }
}